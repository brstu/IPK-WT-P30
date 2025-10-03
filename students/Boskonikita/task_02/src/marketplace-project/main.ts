// Интерфейсы
interface RegistrationData {
    username: string;
    email: string;
    password: string;
    phone: string;
}

interface ValidationResult {
    isValid: boolean;
    message: string;
}

// Валидация username
const validateUsername = (username: string): ValidationResult => {
    if (!username) {
        return { isValid: false, message: 'Имя пользователя обязательно' };
    }

    if (username.length < 3 || username.length > 20) {
        return { isValid: false, message: 'Имя пользователя должно быть от 3 до 20 символов' };
    }

    const usernameRegex = /^[a-zA-Z0-9_]+$/;
    if (!usernameRegex.test(username)) {
        return { isValid: false, message: 'Можно использовать только латиницу, цифры и подчёркивание' };
    }

    if (username.startsWith('_') || username.endsWith('_')) {
        return { isValid: false, message: 'Имя не может начинаться или заканчиваться на "_"' };
    }

    return { isValid: true, message: '' };
};

// Валидация email
const validateEmail = (email: string): ValidationResult => {
    if (!email) {
        return { isValid: false, message: 'Email обязателен для заполнения' };
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        return { isValid: false, message: 'Введите корректный email адрес' };
    }

    return { isValid: true, message: '' };
};

// Валидация пароля
const validatePassword = (password: string): ValidationResult => {
    if (!password) {
        return { isValid: false, message: 'Пароль обязателен для заполнения' };
    }

    if (password.length < 10 || password.length > 64) {
        return { isValid: false, message: 'Пароль должен быть от 10 до 64 символов' };
    }

    if (/\s/.test(password)) {
        return { isValid: false, message: 'Пароль не должен содержать пробелы' };
    }

    if (!/[A-Z]/.test(password)) {
        return { isValid: false, message: 'Пароль должен содержать минимум одну заглавную букву' };
    }

    if (!/[a-z]/.test(password)) {
        return { isValid: false, message: 'Пароль должен содержать минимум одну строчную букву' };
    }

    if (!/\d/.test(password)) {
        return { isValid: false, message: 'Пароль должен содержать минимум одну цифру' };
    }

    if (!/[-_.!@#;]/.test(password)) {
        return { isValid: false, message: 'Пароль должен содержать минимум один спецсимвол: -_.!@#;' };
    }

    return { isValid: true, message: '' };
};

// Валидация телефона (BY формат с операторами)
const validatePhone = (phone: string): ValidationResult => {
    if (!phone) {
        return { isValid: false, message: 'Телефон обязателен для заполнения' };
    }

    const cleanPhone = phone.replace(/[\s-]/g, '');
    const phoneRegex = /^\+375(29|25|33|44)\d{7}$/;

    if (!phoneRegex.test(cleanPhone)) {
        return {
            isValid: false,
            message: 'Введите телефон в формате: +375 (29/25/33/44) XXX-XX-XX'
        };
    }

    return { isValid: true, message: '' };
};

// Полная валидация формы
const validateForm = (formData: RegistrationData): boolean =>
    validateUsername(formData.username).isValid &&
    validateEmail(formData.email).isValid &&
    validatePassword(formData.password).isValid &&
    validatePhone(formData.phone).isValid;

// Основной класс формы
class RegistrationForm {
    private form: HTMLFormElement;
    private usernameInput: HTMLInputElement;
    private emailInput: HTMLInputElement;
    private passwordInput: HTMLInputElement;
    private phoneInput: HTMLInputElement;

    constructor() {
        this.form = document.getElementById('registrationForm') as HTMLFormElement;
        this.usernameInput = document.getElementById('username') as HTMLInputElement;
        this.emailInput = document.getElementById('email') as HTMLInputElement;
        this.passwordInput = document.getElementById('password') as HTMLInputElement;
        this.phoneInput = document.getElementById('phone') as HTMLInputElement;

        this.initializeEventListeners();
    }

    private initializeEventListeners(): void {
        this.usernameInput.addEventListener('input', () => this.validateField('username'));
        this.emailInput.addEventListener('input', () => this.validateField('email'));
        this.passwordInput.addEventListener('input', () => this.validateField('password'));
        this.phoneInput.addEventListener('input', () => this.validateField('phone'));

        this.form.addEventListener('submit', (e) => this.handleSubmit(e));
    }

    private validateField(fieldName: keyof RegistrationData): void {
        let result: ValidationResult;
        const errorElement = document.getElementById(`${fieldName}-error`) as HTMLDivElement;

        switch (fieldName) {
            case 'username':
                result = validateUsername(this.usernameInput.value);
                break;
            case 'email':
                result = validateEmail(this.emailInput.value);
                break;
            case 'password':
                result = validatePassword(this.passwordInput.value);
                break;
            case 'phone':
                result = validatePhone(this.phoneInput.value);
                break;
        }

        this.updateFieldValidation(fieldName, result, errorElement);
    }

    private updateFieldValidation(
        fieldName: keyof RegistrationData,
        result: ValidationResult,
        errorElement: HTMLDivElement
    ): void {
        const inputElement = document.getElementById(fieldName) as HTMLInputElement;

        if (result.isValid) {
            errorElement.textContent = '';
            inputElement.classList.remove('invalid');
            inputElement.classList.add('valid');
        } else {
            errorElement.textContent = result.message;
            inputElement.classList.remove('valid');
            inputElement.classList.add('invalid');
        }
    }

    private handleSubmit(event: Event): void {
        event.preventDefault();

        this.validateField('username');
        this.validateField('email');
        this.validateField('password');
        this.validateField('phone');

        const formData: RegistrationData = {
            username: this.usernameInput.value,
            email: this.emailInput.value,
            password: this.passwordInput.value,
            phone: this.phoneInput.value
        };

        if (validateForm(formData)) {
            alert('Регистрация успешно завершена!');
            console.log('Данные формы:', formData);
            this.form.reset();
            this.clearValidationStates();
        }
    }

    private clearValidationStates(): void {
        const inputs = [this.usernameInput, this.emailInput, this.passwordInput, this.phoneInput];
        const errorElements = document.querySelectorAll<HTMLDivElement>('.error-message');

        inputs.forEach((input) => {
            input.classList.remove('valid', 'invalid');
        });

        errorElements.forEach((element) => {
            element.textContent = '';
        });
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new RegistrationForm();
});
