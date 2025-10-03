// Основные JavaScript функции для Task05

// Подтверждение действий
function confirmAction(message) {
    return confirm(message || 'Вы уверены, что хотите выполнить это действие?');
}

// Отображение уведомлений
function showNotification(message, type = 'info') {
    // Реализация показа уведомлений
    console.log(`${type}: ${message}`);
}

// Валидация форм
function validateFileInput(input) {
    const file = input.files[0];
    if (!file) return true;
    
    const maxSize = 2 * 1024 * 1024; // 2MB
    const allowedTypes = ['image/jpeg', 'image/png', 'application/pdf', 
                         'application/msword', 
                         'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];
    
    if (file.size > maxSize) {
        alert('Файл слишком большой. Максимальный размер: 2MB');
        input.value = '';
        return false;
    }
    
    if (!allowedTypes.includes(file.type)) {
        alert('Недопустимый тип файла. Разрешены: JPG, PNG, PDF, DOC, DOCX');
        input.value = '';
        return false;
    }
    
    return true;
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    // Инициализация компонентов
    initializeFileUploads();
});

function initializeFileUploads() {
    const fileInputs = document.querySelectorAll('input[type="file"]');
    fileInputs.forEach(input => {
        input.addEventListener('change', function() {
            validateFileInput(this);
        });
    });
}
