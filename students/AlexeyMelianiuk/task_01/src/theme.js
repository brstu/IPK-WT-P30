// Theme switching functionality
class ThemeManager {
    constructor() {
        this.themeToggle = document.querySelector('.theme-toggle');
        this.currentTheme = localStorage.getItem('theme') || 'light';
        
        this.init();
    }
    
    init() {
        this.applyTheme(this.currentTheme);
        this.setupEventListeners();
    }
    
    setupEventListeners() {
        if (this.themeToggle) {
            this.themeToggle.addEventListener('click', () => this.toggleTheme());
            this.themeToggle.addEventListener('keydown', (e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    this.toggleTheme();
                }
            });
        }
    }
    
    toggleTheme() {
        this.currentTheme = this.currentTheme === 'light' ? 'dark' : 'light';
        this.applyTheme(this.currentTheme);
        this.updateToggleText();
        localStorage.setItem('theme', this.currentTheme);
    }
    
    applyTheme(theme) {
        document.documentElement.setAttribute('data-theme', theme);
        
        // Update meta theme-color for mobile browsers
        const metaThemeColor = document.querySelector('meta[name="theme-color"]');
        if (metaThemeColor) {
            metaThemeColor.setAttribute('content', theme === 'dark' ? '#1a202c' : '#4a6fa5');
        }
    }
    
    updateToggleText() {
        if (this.themeToggle) {
            const icon = this.currentTheme === 'light' ? '🌙' : '☀️';
            const text = this.currentTheme === 'light' ? 'Тёмная тема' : 'Светлая тема';
            this.themeToggle.innerHTML = `${icon} ${text}`;
            this.themeToggle.setAttribute('aria-label', 
                this.currentTheme === 'light' ? 'Включить тёмную тему' : 'Включить светлую тему');
        }
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new ThemeManager();
});

// Form validation enhancement
document.addEventListener('DOMContentLoaded', () => {
    const forms = document.querySelectorAll('form');
    
    forms.forEach(form => {
        form.addEventListener('submit', (e) => {
            if (!form.checkValidity()) {
                e.preventDefault();
                // Focus on first invalid field
                const invalidField = form.querySelector(':invalid');
                if (invalidField) {
                    invalidField.focus();
                }
            }
        });
        
        // Add real-time validation feedback
        const inputs = form.querySelectorAll('input, textarea, select');
        inputs.forEach(input => {
            input.addEventListener('blur', () => {
                validateField(input);

});
        });
    });
    
    function validateField(field) {
        const isValid = field.checkValidity();
        const messageId = field.getAttribute('aria-describedby');
        const messageElement = messageId ? document.getElementById(messageId) : null;
        
        if (isValid) {
            field.classList.remove('invalid');
            field.classList.add('valid');
            if (messageElement) {
                messageElement.style.color = '';
            }
        } else {
            field.classList.remove('valid');
            field.classList.add('invalid');
            if (messageElement) {
                messageElement.style.color = 'var(--accent-color)';
            }
        }
    }
});

// Add CSS for validation states
const style = document.createElement('style');
style.textContent = `
    input.valid, textarea.valid, select.valid {
        border-color: var(--secondary-color) !important;
    }
    
    input.invalid, textarea.invalid, select.invalid {
        border-color: var(--accent-color) !important;
    }
`;
document.head.appendChild(style);