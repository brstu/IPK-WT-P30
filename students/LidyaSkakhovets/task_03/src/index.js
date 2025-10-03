// Конфигурация
const API_URL = 'https://jsonplaceholder.typicode.com/users';
const CACHE_KEY = 'lr03_users_cache';
const CACHE_TTL_MS = 5 * 60 * 1000; // 5 минут
const PAGE_SIZE = 5;

// Элементы DOM
const elements = {
    status: document.getElementById('status'),
    tableBody: document.getElementById('table-body'),
    prevBtn: document.getElementById('prev'),
    nextBtn: document.getElementById('next'),
    pageInfo: document.getElementById('page-info'),
    clearCacheBtn: document.getElementById('clear-cache'),
    cacheInfo: document.getElementById('cache-info')
};

// Состояние приложения
const state = {
    data: [],
    currentPage: 1,
    totalPages: 1,
    isLoading: false
};

// Утилиты для работы со статусом
function setStatus(message, type = 'loading') {
    elements.status.textContent = message;
    elements.status.className = '';
    
    switch (type) {
        case 'loading':
            elements.status.classList.add('status-loading');
            break;
        case 'success':
            elements.status.classList.add('status-success');
            break;
        case 'error':
            elements.status.classList.add('status-error');
            break;
        case 'cache':
            elements.status.classList.add('status-cache');
            break;
    }
}

function setLoading(loading) {
    state.isLoading = loading;
    if (loading) {
        document.body.style.opacity = '0.7';
        document.body.style.pointerEvents = 'none';
    } else {
        document.body.style.opacity = '1';
        document.body.style.pointerEvents = 'auto';
    }
}

// Работа с API
async function fetchDataFromAPI() {
    setStatus('Загрузка данных с сервера...', 'loading');
    setLoading(true);
    
    try {
        const response = await fetch(API_URL);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        setStatus('Данные успешно загружены с сервера', 'success');
        return data;
        
    } catch (error) {
        console.error('Ошибка при загрузке данных:', error);
        setStatus(`Ошибка загрузки: ${error.message}`, 'error');
        throw error;
    } finally {
        setLoading(false);
    }
}

// Работа с кэшем
function getCachedData() {
    try {
        const cached = localStorage.getItem(CACHE_KEY);
        if (!cached) return null;
        
        const parsed = JSON.parse(cached);
        const now = Date.now();
        
        // Проверка TTL
        if (now - parsed.timestamp > CACHE_TTL_MS) {
            localStorage.removeItem(CACHE_KEY);
            return null;
        }
        
        return parsed.data;
    } catch (error) {
        console.error('Ошибка при чтении кэша:', error);
        return null;
    }
}

function setCacheData(data) {
    try {
        const cacheObject = {
            data: data,
            timestamp: Date.now(),
            totalCount: data.length
        };
        localStorage.setItem(CACHE_KEY, JSON.stringify(cacheObject));
        updateCacheInfo();
    } catch (error) {
        console.error('Ошибка при записи в кэш:', error);
    }
}

function clearCache() {
    localStorage.removeItem(CACHE_KEY);
    setStatus('Кэш очищен', 'success');
    updateCacheInfo();
    // Перезагружаем данные
    loadData();
}

function updateCacheInfo() {
    const cached = getCachedData();
    if (cached) {
        const cacheTime = JSON.parse(localStorage.getItem(CACHE_KEY)).timestamp;
        const timeDiff = Date.now() - cacheTime;
        const minutesLeft = Math.max(0, Math.floor((CACHE_TTL_MS - timeDiff) / 60000));
        elements.cacheInfo.textContent = `Кэш: ${cached.length} записей (актуален еще ${minutesLeft} мин)`;
    } else {
        elements.cacheInfo.textContent = 'Кэш пуст';
    }
}

// Работа с таблицей и пагинацией
function renderTable() {
    if (state.data.length === 0) {
        elements.tableBody.innerHTML = `
            <tr>
                <td colspan="5" class="loading-text">Нет данных для отображения</td>
            </tr>
        `;
        return;
    }
    
    const startIndex = (state.currentPage - 1) * PAGE_SIZE;
    const endIndex = startIndex + PAGE_SIZE;
    const pageData = state.data.slice(startIndex, endIndex);
    
    elements.tableBody.innerHTML = pageData.map(user => `
        <tr>
            <td>${user.id}</td>
            <td>${user.name}</td>
            <td>${user.email}</td>
            <td>${user.address.city}</td>
            <td>${user.phone}</td>
        </tr>
    `).join('');
}

function updatePagination() {
    state.totalPages = Math.ceil(state.data.length / PAGE_SIZE);
    
    // Корректируем текущую страницу если нужно
    if (state.currentPage > state.totalPages) {
        state.currentPage = Math.max(1, state.totalPages);
    }
    
    elements.pageInfo.textContent = `Страница ${state.currentPage}/${state.totalPages}`;
    elements.prevBtn.disabled = state.currentPage <= 1 || state.isLoading;
    elements.nextBtn.disabled = state.currentPage >= state.totalPages || state.isLoading;
}

function goToPreviousPage() {
    if (state.currentPage > 1 && !state.isLoading) {
        state.currentPage--;
        renderTable();
        updatePagination();
    }
}

function goToNextPage() {
    if (state.currentPage < state.totalPages && !state.isLoading) {
        state.currentPage++;
        renderTable();
        updatePagination();
    }
}

// Основная логика
async function loadData() {
    try {
        // Пробуем получить данные из кэша
        let data = getCachedData();
        
        if (data) {
            setStatus('Данные загружены из кэша', 'cache');
        } else {
            // Если в кэше нет данных, загружаем с API
            data = await fetchDataFromAPI();
            setCacheData(data);
        }
        
        state.data = data;
        state.currentPage = 1;
        
        renderTable();
        updatePagination();
        updateCacheInfo();
        
    } catch (error) {
        state.data = [];
        renderTable();
        updatePagination();
    }
}

// Инициализация приложения
function init() {
    // Назначаем обработчики событий
    elements.prevBtn.addEventListener('click', goToPreviousPage);
    elements.nextBtn.addEventListener('click', goToNextPage);
    elements.clearCacheBtn.addEventListener('click', clearCache);
    
    // Загружаем данные
    loadData();
}

// Запускаем приложение когда DOM загружен
document.addEventListener('DOMContentLoaded', init);