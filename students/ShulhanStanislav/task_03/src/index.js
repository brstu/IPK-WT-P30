// Константы API и кэширования
const API_URL = 'https://jsonplaceholder.typicode.com/todos';
const CACHE_KEY = 'lr03_finance_cache';
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
        const response = await fetch(API_URL);
        
        if (!response.ok) {
            throw new Error(`Ошибка сети: ${response.status} ${response.statusText}`);
        }

        const apiData = await response.json();
        console.log('Data received from API:', apiData);

        // Преобразуем данные API под нашу финансовую тему
        const financialData = apiData.map(item => ({
            id: item.id,
            category: item.title,
            amount: item.completed ? 100 : 50, // completed -> amount
            status: item.completed ? 'Оплачено' : 'Ожидание' // completed -> status
        }));

        return financialData;
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
        tbody.innerHTML = '<tr><td colspan="4" style="text-align: center;">Нет данных для отображения</td></tr>';
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
    pageData.forEach(item => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${item.id}</td>
            <td>${item.category}</td>
            <td>${item.amount}</td>
            <td>${item.status}</td>
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
        let financialData = readCache();

        if (financialData && financialData.length > 0) {
            setStatus('Данные успешно загружены из кэша (актуальны до 5 минут).', 'success');
        } else {
            // Если в кэше нет данных или они устарели, загружаем из API
            financialData = await fetchFromApi();
            writeCache(financialData); // Сохраняем новые данные в кэш
            setStatus('Данные успешно загружены с сервера и сохранены в кэш.', 'success');
        }

        // Сохраняем данные в состоянии приложения
        state.data = financialData;
        // Отрисовываем таблицу
        renderTable();

    } catch (error) {
        console.error('Initialization error:', error);
        setError(`Не удалось загрузить данные: ${error.message}`);
        
        // Показываем заглушку при ошибке
        tbody.innerHTML = `
            <tr>
                <td colspan="4" style="text-align: center; color: #e74c3c;">
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