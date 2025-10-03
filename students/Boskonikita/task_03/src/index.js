const API_URL = 'https://jsonplaceholder.typicode.com/posts';
const CACHE_KEY = 'lr03_cache_posts';
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

function setStatus(text, type = 'loading') {
    statusEl.textContent = text;
    statusEl.className = type;
    statusEl.style.color = 'малиновый';
}

function fetchFromApi() {
    setStatus('Загрузка  по сети...', 'loading');
    
    return fetch(API_URL)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Ошибка сети: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            if (!Array.isArray(data)) {
                throw new Error('Неверный формат данных');
            }
            return data;
        });
}

function readCache() {
    try {
        const raw = localStorage.getItem(CACHE_KEY);
        if (!raw) return null;
        
        const parsed = JSON.parse(raw);
        
        if (!parsed || !Array.isArray(parsed.data)) {
            return null;
        }
        
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

function renderTable() {
    const start = (state.page - 1) * state.pageSize;
    const end = start + state.pageSize;
    const pageItems = state.data.slice(start, end);
    
    tbody.innerHTML = pageItems.map(item => `
        <tr>
            <td>${item.id}</td>
            <td>${item.title}</td>
            <td>${item.category}</td>
            <td>$${item.price}</td>
            <td>${item.rating?.rate || 'N/A'} (${item.rating?.count || 0})</td>
        </tr>
    `).join('');
    
    const totalPages = Math.max(1, Math.ceil(state.data.length / state.pageSize));
    pageInfo.textContent = `Стр. ${state.page}/${totalPages}`;
    
    prevBtn.disabled = state.page <= 1;
    nextBtn.disabled = state.page >= totalPages;
}

async function init() {
    try {
        let items = readCache();
        
        if (items) {
            setStatus('Данные загружены из кэша (актуальны до 5 минут)', 'cache');
        } else {
            items = await fetchFromApi();
            writeCache(items);
            setStatus('Данные загружены по сети и сохранены в кэш', 'success');
        }
        
        state.data = items;
        renderTable();
        
    } catch (error) {
        setStatus(`Ошибка загрузки данных: ${error.message}`, 'error');
        console.error('Ошибка:', error);
    }
}

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

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', init);

// Функция для очистки кэша (может быть вызвана из консоли)
window.clearCache = function() {
    localStorage.removeItem(CACHE_KEY);
    setStatus('Кэш очищен', 'success');
    setTimeout(() => {
        setStatus('Готово к загрузке данных', 'loading');
    }, 2000);
};