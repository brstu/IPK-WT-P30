import { FormData, ValidationResult } from './form-types';

// Регулярные выражения для валидации
export const PHONE_REGEX = /^\+\d{10,15}$/;
export const SPECIAL_CHARS_REGEX = /[@#$%&]/;
export const DIGIT_REGEX = /\d/g;
export const NAME_REGEX = /^[a-zA-Zа-яА-ЯёЁ\s\-]{2,40}$/;

export class FormValidator {
  static validateFullName(fullName: string): string | null {
    if (!fullName) {
      return 'ФИО обязательно для заполнения';
    }

    if (!NAME_REGEX.test(fullName)) {
      return 'ФИО должно содержать 2-40 символов (кириллица, латиница, пробелы и дефисы)';
    }

    return null;
  }

  static validateEmail(email: string): string | null {
    if (!email) {
      return 'Email обязателен для заполнения';
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      return 'Введите корректный email адрес';
    }

    return null;
  }

  static validatePassword(password: string): string | null {
    if (!password) {
      return 'Пароль обязателен для заполнения';
    }

    // Проверка специальных символов
    if (!SPECIAL_CHARS_REGEX.test(password)) {
      return 'Пароль должен содержать минимум один специальный символ (@, #, $, %, &)';
    }

    // Проверка цифр
    const digits = password.match(DIGIT_REGEX);
    if (!digits || digits.length < 2) {
      return 'Пароль должен содержать минимум две цифры';
    }

    return null;
  }

  static validatePhone(phone: string): string | null {
    if (!phone) {
      return 'Телефон обязателен для заполнения';
    }

    // Нормализация: удаляем все пробелы для проверки
    const normalizedPhone = phone.replace(/\s/g, '');
    
    if (!PHONE_REGEX.test(normalizedPhone)) {
      return 'Введите корректный номер телефона в формате +XXXXXXXXXXX (10-15 цифр после +)';
    }

    return null;
  }

  static validateForm(formData: FormData): ValidationResult {
    const errors = {
      fullName: this.validateFullName(formData.fullName),
      email: this.validateEmail(formData.email),
      password: this.validatePassword(formData.password),
      phone: this.validatePhone(formData.phone),
    };

    const isValid = Object.values(errors).every((error) => error === null);

    return {
      isValid,
      errors: errors as { [key: string]: string | undefined },
    };
  }
}