// Константы API и кэширования
const USERS_API_URL = 'https://jsonplaceholder.typicode.com/users';
const POSTS_API_URL = 'https://jsonplaceholder.typicode.com/posts';
const COMMENTS_API_URL = 'https://jsonplaceholder.typicode.com/comments';
const CACHE_KEY = 'lr03_social_cache';
const CACHE_TTL_MS = 5 * 60 * 1000; // 5 минут

// Получаем элементы DOM
const statusEl = document.getElementById('status');
const tbody = document.getElementById('table-body');
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');
const pageInfo = document.getElementById('page-info');
const clearCacheBtn = document.getElementById('clear-cache');

// Состояние приложения
const state = {
    data: [],
    page: 1,
    pageSize: 10 // Количество элементов на странице
};

// Функции для работы со статусом
function setStatus(text, type = 'info') {
    statusEl.textContent = text;
    statusEl.className = ''; // Сбрасываем классы
    if (type === 'error') statusEl.classList.add('error');
    if (type === 'success') statusEl.classList.add('success');
    console.log('Status:', text);
}

function setError(text) {
    setStatus(text, 'error');
}

// Функция для получения данных из API
async function fetchFromApi() {
    setStatus('Загрузка актуальных данных с сервера...', 'info');
    try {
        // Загружаем данные параллельно для оптимизации
        const [usersResponse, postsResponse, commentsResponse] = await Promise.all([
            fetch(USERS_API_URL),
            fetch(POSTS_API_URL),
            fetch(COMMENTS_API_URL)
        ]);
        
        if (!usersResponse.ok || !postsResponse.ok || !commentsResponse.ok) {
            throw new Error(`Ошибка сети: ${usersResponse.status}`);
        }

        const users = await usersResponse.json();
        const posts = await postsResponse.json();
        const comments = await commentsResponse.json();
        console.log('Data received from API:', { users, posts, comments });

        // Считаем статистику для каждого пользователя
        const postsCountByUser = posts.reduce((acc, post) => {
            acc[post.userId] = (acc[post.userId] || 0) + 1;
            return acc;
        }, {});

        const commentsCountByUser = comments.reduce((acc, comment) => {
            // Для комментариев связь с пользователем через email (в реальном API был бы userId)
            const user = users.find(u => u.email === comment.email);
            if (user) {
                acc[user.id] = (acc[user.id] || 0) + 1;
            }
            return acc;
        }, {});

        // Преобразуем данные для таблицы
        const socialData = users.map(user => ({
            id: user.id,
            name: user.name,
            email: user.email,
            postsCount: postsCountByUser[user.id] || 0,
            commentsCount: commentsCountByUser[user.id] || 0
        }));

        return socialData;
    } catch (error) {
        console.error('Fetch error:', error);
        throw error;
    }
}

// Функции для работы с кэшем (localStorage)
function readCache() {
    try {
        const rawData = localStorage.getItem(CACHE_KEY);
        if (!rawData) {
            console.log('Кэш не найден');
            return null;
        }

        const parsedData = JSON.parse(rawData);
        // Проверяем, не устарели ли данные (TTL)
        if (Date.now() - parsedData.timestamp > CACHE_TTL_MS) {
            console.log('Кэш устарел');
            return null;
        }
        console.log('Данные загружены из кэша');
        return parsedData.data;
    } catch (error) {
        console.error('Ошибка чтения кэша:', error);
        return null;
    }
}

function writeCache(data) {
    try {
        const cacheObject = {
            data: data,
            timestamp: Date.now()
        };
        localStorage.setItem(CACHE_KEY, JSON.stringify(cacheObject));
        console.log('Данные сохранены в кэш');
    } catch (error) {
        console.error('Ошибка записи в кэш:', error);
    }
}

// Отрисовка таблицы и пагинации
function renderTable() {
    console.log('Rendering table, data length:', state.data.length);
    
    if (state.data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" style="text-align: center;">Нет данных для отображения</td></tr>';
        pageInfo.textContent = 'Стр. 0/0';
        prevBtn.disabled = true;
        nextBtn.disabled = true;
        return;
    }
    
    // Вычисляем элементы для текущей страницы
    const startIndex = (state.page - 1) * state.pageSize;
    const endIndex = startIndex + state.pageSize;
    const pageData = state.data.slice(startIndex, endIndex);

    // Очищаем тело таблицы
    tbody.innerHTML = '';

    // Заполняем таблицу данными
    pageData.forEach(user => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${user.id}</td>
            <td>${user.name}</td>
            <td>${user.email}</td>
            <td>${user.postsCount}</td>
            <td>${user.commentsCount}</td>
        `;
        tbody.appendChild(row);
    });

    // Обновляем информацию о страницах
    const totalPages = Math.ceil(state.data.length / state.pageSize);
    pageInfo.textContent = `Стр. ${state.page}/${totalPages}`;

    // Блокируем/разблокируем кнопки навигации
    prevBtn.disabled = state.page <= 1;
    nextBtn.disabled = state.page >= totalPages;
}

// Основная функция инициализации
async function init() {
    console.log('Initializing application...');
    try {
        // Пытаемся загрузить данные из кэша
        let socialData = readCache();

        if (socialData && socialData.length > 0) {
            setStatus('Данные успешно загружены из кэша (актуальны до 5 минут).', 'success');
        } else {
            // Если в кэше нет данных или они устарели, загружаем из API
            socialData = await fetchFromApi();
            writeCache(socialData); // Сохраняем новые данные в кэш
            setStatus('Данные успешно загружены с сервера и сохранены в кэш.', 'success');
        }

        // Сохраняем данные в состоянии приложения
        state.data = socialData;
        // Отрисовываем таблицу
        renderTable();

    } catch (error) {
        console.error('Initialization error:', error);
        setError(`Не удалось загрузить данные: ${error.message}`);
        
        // Показываем заглушку при ошибке
        tbody.innerHTML = `
            <tr>
                <td colspan="5" style="text-align: center; color: #e74c3c;">
                    Ошибка загрузки данных. Проверьте подключение к интернету.
                </td>
            </tr>
        `;
    }
}

// Обработчики событий
prevBtn.addEventListener('click', () => {
    state.page--;
    renderTable();
});

nextBtn.addEventListener('click', () => {
    state.page++;
    renderTable();
});

clearCacheBtn.addEventListener('click', () => {
    localStorage.removeItem(CACHE_KEY);
    setStatus('Кэш успешно очищен. Для обновления данных перезагрузите страницу.', 'info');
});

// Запускаем приложение после полной загрузки DOM
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
} else {
    init();
}