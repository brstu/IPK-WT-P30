const API_URL = 'https://jsonplaceholder.typicode.com/posts';
const CACHE_KEY = 'lr03cacheposts';
const CACHE_TTL_MS = 5 * 60 * 1000; // 5 минут

const statusEl = document.getElementById('status');
const tbody = document.getElementById('table-body');
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');
const pageInfo = document.getElementById('page-info');

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

function setError(text) {
    setStatus(text, 'error');
}

// Загрузка данных из API[citation:3]
async function fetchFromApi() {
    setStatus('Загрузка по сети…', 'loading');
    try {
        const response = await fetch(API_URL);
        if (!response.ok) {
            throw new Error(`Ошибка сети: ${response.status}`);
        }
        const data = await response.json();
        return data;
    } catch (error) {
        throw new Error(`Не удалось загрузить данные: ${error.message}`);
    }
}

// Чтение из кэша[citation:2]
function readCache() {
    const raw = localStorage.getItem(CACHE_KEY);
    if (!raw) return null;
    
    try {
        const parsed = JSON.parse(raw);
        if (!parsed || !Array.isArray(parsed.data)) return null;
        
        // Проверяем, не устарели ли данные
        if (Date.now() - parsed.cachedAt > CACHE_TTL_MS) {
            return null;
        }
        
        return parsed.data;
    } catch (error) {
        return null;
    }
}

// Запись в кэш[citation:2]
function writeCache(items) {
    const cacheData = {
        data: items,
        cachedAt: Date.now()
    };
    localStorage.setItem(CACHE_KEY, JSON.stringify(cacheData));
}

// Отрисовка таблицы[citation:4]
function renderTable() {
    const start = (state.page - 1) * state.pageSize;
    const end = start + state.pageSize;
    const pageItems = state.data.slice(start, end);
    
    // Очищаем таблицу
    tbody.innerHTML = '';
    
    // Заполняем таблицу данными
    pageItems.forEach(item => {
        const row = document.createElement('tr');
        const cellId = document.createElement('td');
        cellId.textContent = item.id;
const cellTitle = document.createElement('td');
        cellTitle.textContent = item.title;
        
        const cellBody = document.createElement('td');
        cellBody.textContent = item.body;
        
        row.appendChild(cellId);
        row.appendChild(cellTitle);
        row.appendChild(cellBody);
        
        tbody.appendChild(row);
    });
    
    // Обновляем информацию о страницах
    const totalPages = Math.max(1, Math.ceil(state.data.length / state.pageSize));
    pageInfo.textContent = `Стр. ${state.page}/${totalPages}`;
    
    // Блокируем кнопки на границах[citation:4]
    prevBtn.disabled = state.page <= 1;
    nextBtn.disabled = state.page >= totalPages;
}

// Инициализация приложения
async function init() {
    try {
        let items = readCache();
        
        if (items) {
            setStatus('Данные загружены из кэша (актуальны до 5 минут)', 'success');
        } else {
            items = await fetchFromApi();
            writeCache(items);
            setStatus('Данные загружены по сети и сохранены в кэш', 'success');
        }
        
        state.data = items;
        renderTable();
        
    } catch (error) {
        setError(error.message);
    }
}

// Обработчики событий для кнопок пагинации[citation:4]
prevBtn.addEventListener('click', () => {
    if (state.page > 1) {
        state.page--;
        renderTable();
    }
});

nextBtn.addEventListener('click', () => {
    const totalPages = Math.ceil(state.data.length / state.pageSize);
    if (state.page < totalPages) {
        state.page++;
        renderTable();
    }
});

// Функция для очистки кэша (может быть вызвана из консоли)
function clearCache() {
    localStorage.removeItem(CACHE_KEY);
    setStatus('Кэш очищен', 'info');
}

// Запускаем приложение
init();