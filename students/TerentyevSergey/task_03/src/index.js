const API_URL = 'https://jsonplaceholder.typicode.com/posts';
const CACHE_KEY = 'lr03_cache_posts';
const CACHE_TTL_MS = 5 * 60 * 1000; // 5 минут

// DOM элементы
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
    pageSize: 10
};

// Функции для работы со статусом
function setStatus(text, type = 'info') {
    statusEl.textContent = text;
    statusEl.className = type;
}

function setLoading() {
    setStatus('Загрузка данных...', 'loading');
}

function setError(text) {
    setStatus(text, 'error');
}

// Работа с API
async function fetchFromApi() {
    setLoading();
    try {
        const response = await fetch(API_URL);
        
        if (!response.ok) {
            throw new Error(`Ошибка сети: ${response.status} ${response.statusText}`);
        }
        
        const data = await response.json();
        return data;
    } catch (error) {
        throw new Error(`Не удалось загрузить данные: ${error.message}`);
    }
}

// Работа с кэшем
function readCache() {
    try {
        const raw = localStorage.getItem(CACHE_KEY);
        if (!raw) return null;

        const parsed = JSON.parse(raw);
        
        if (!parsed || !Array.isArray(parsed.data)) {
            return null;
        }

        // Проверка TTL
        if (Date.now() - parsed.cachedAt > CACHE_TTL_MS) {
            localStorage.removeItem(CACHE_KEY);
            return null;
        }

        return parsed.data;
    } catch (error) {
        console.error('Ошибка чтения кэша:', error);
        return null;
    }
}

function writeCache(items) {
    try {
        const cacheData = {
            data: items,
            cachedAt: Date.now()
        };
        localStorage.setItem(CACHE_KEY, JSON.stringify(cacheData));
    } catch (error) {
        console.error('Ошибка записи в кэш:', error);
    }
}

function clearCache() {
    localStorage.removeItem(CACHE_KEY);
    setStatus('Кэш очищен', 'info');
}

// Отрисовка таблицы
function renderTable() {
    const startIndex = (state.page - 1) * state.pageSize;
    const endIndex = startIndex + state.pageSize;
    const pageItems = state.data.slice(startIndex, endIndex);

    tbody.innerHTML = pageItems.map(item => `
        <tr>
            <td>${item.id}</td>
            <td>${item.title}</td>
            <td>${item.body}</td>
            <td>${item.userId}</td>
        </tr>
    `).join('');
}

// Пагинация
function updatePagination() {
    const totalPages = Math.ceil(state.data.length / state.pageSize);
    
    pageInfo.textContent = `Стр. ${state.page}/${totalPages}`;
    prevBtn.disabled = state.page <= 1;
    nextBtn.disabled = state.page >= totalPages;
}

function renderPage() {
    renderTable();
    updatePagination();
}

// Навигация
function goToPrevPage() {
    if (state.page > 1) {
        state.page--;
        renderPage();
    }
}

function goToNextPage() {
    const totalPages = Math.ceil(state.data.length / state.pageSize);
    if (state.page < totalPages) {
        state.page++;
        renderPage();
    }
}

// Инициализация приложения
async function init() {
    try {
        // Пробуем прочитать из кэша
        let items = readCache();
        
        if (items) {
            setStatus('Данные загружены из кэша (актуальны до 5 минут)', 'info');
        } else {
            // Если кэша нет или устарел - загружаем по сети
            items = await fetchFromApi();
            writeCache(items);
            setStatus('Данные загружены по сети и сохранены в кэш', 'info');
        }

        state.data = items;
        renderPage();

    } catch (error) {
        setError(error.message);
        console.error('Ошибка инициализации:', error);
    }
}

// Обработчики событий
prevBtn.addEventListener('click', goToPrevPage);
nextBtn.addEventListener('click', goToNextPage);
clearCacheBtn.addEventListener('click', () => {
    clearCache();
    // Перезагружаем данные после очистки кэша
    init();
});

// Запуск приложения
document.addEventListener('DOMContentLoaded', init);