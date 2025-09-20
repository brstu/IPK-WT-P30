import { ValidationRules, FormData, ValidationResult } from './types';

export const validationRules: ValidationRules = {
    email: (value: string): string | null => {
        if (!value.trim()) {
            return 'Email обязателен для заполнения';
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            return 'Введите корректный email адрес';
        }

        return null;
    },

    phone: (value: string): string | null => {
        if (!value.trim()) {
            return 'Телефон обязателен для заполнения';
        }

        const phoneRegex = /^\+375\s?(?:\(?\d{2}\)?[\s-]?)?\d{3}[\s-]?\d{2}[\s-]?\d{2}$/;
        
        if (!phoneRegex.test(value)) {
            return 'Телефон должен быть в формате +375 XX XXX-XX-XX';
        }

        const operatorCode = value.replace(/\D/g, '').substring(4, 6);
        const validOperators = ['29', '33', '44', '25'];
        
        if (!validOperators.includes(operatorCode)) {
            return 'Неверный код оператора. Допустимые: 29, 33, 44, 25';
        }

        return null;
    },

    password: (value: string): string | null => {
        if (!value.trim()) {
            return 'Пароль обязателен для заполнения';
        }

        if (value.length < 8) {
            return 'Пароль должен содержать минимум 8 символов';
        }

        const hasUpperCase = /[A-Z]/.test(value);
        if (!hasUpperCase) {
            return 'Пароль должен содержать хотя бы одну заглавную букву';
        }

        const hasLowerCase = /[a-z]/.test(value);
        if (!hasLowerCase) {
            return 'Пароль должен содержать хотя бы одну строчную букву';
        }

        const hasNumber = /\d/.test(value);
        if (!hasNumber) {
            return 'Пароль должен содержать хотя бы одну цифру';
        }

        return null;
    },

    skills: (values: string[]): string | null => {
        if (values.length === 0) {
            return 'Выберите хотя бы один навык';
        }

        return null;
    }
};

export const validateForm = (formData: FormData): ValidationResult => {
    const errors = {
        email: validationRules.email(formData.email),
        phone: validationRules.phone(formData.phone),
        password: validationRules.password(formData.password),
        skills: validationRules.skills(formData.skills)
    };

    const isValid = Object.values(errors).every(error => error === null);

    const formattedErrors = {
        email: errors.email || undefined,
        phone: errors.phone || undefined,
        password: errors.password || undefined,
        skills: errors.skills || undefined
    };

    return {
        isValid,
        errors: formattedErrors
    };
};

export const formatPhone = (value: string): string => {
    const cleaned = value.replace(/\D/g, '');
    
    if (cleaned.startsWith('375')) {
        return `+${cleaned}`;
    }
    
    if (cleaned.length === 9) {
        return `+375 ${cleaned}`;
    }
    
    return value;
};