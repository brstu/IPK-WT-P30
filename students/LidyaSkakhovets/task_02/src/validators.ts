import { FieldValidator } from '../types/validation';

export class EmailValidator implements FieldValidator {
  validate(email: string): string | null {
    if (!email.trim()) {
      return 'Email обязателен для заполнения';
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      return 'Введите корректный email адрес';
    }

    return null;
  }
}

export class PasswordValidator implements FieldValidator {
  validate(password: string): string | null {
    if (!password.trim()) {
      return 'Пароль обязателен для заполнения';
    }

    if (password.length < 6) {
      return 'Пароль должен содержать минимум 6 символов';
    }

    // Проверка на запрещенные слова (без учета регистра)
    const forbiddenWordsRegex = /password|пароль/i;
    if (forbiddenWordsRegex.test(password)) {
      return 'Пароль не должен содержать слова "password" или "пароль"';
    }

    return null;
  }
}

export class PhoneValidator implements FieldValidator {
  validate(phone: string): string | null {
    if (!phone.trim()) {
      return 'Телефон обязателен для заполнения';
    }

    // Форматы для RU: +7..., 8..., для BY: +375...
    const phoneRegex = /^(\+7|8)\d{10}$|^(\+375)\d{9}$/;
    
    // Удаляем все нецифровые символы кроме +
    const cleanPhone = phone.replace(/[^\d+]/g, '');

    if (!phoneRegex.test(cleanPhone)) {
      return 'Введите корректный номер телефона (RU/BY)';
    }

    return null;
  }
}

export class LibraryCardValidator implements FieldValidator {
  validate(libraryCard: string): string | null {
    if (!libraryCard.trim()) {
      return 'Номер читательского билета обязателен';
    }

    // Формат: LC‑2025‑ + 5 цифр
    const libraryCardRegex = /^LC‑2025‑\d{5}$/;
    
    if (!libraryCardRegex.test(libraryCard)) {
      return 'Формат: LC‑2025‑XXXXX (5 цифр)';
    }

    return null;
  }
}

export class FormValidator {
  private validators: Map<string, FieldValidator>;

  constructor() {
    this.validators = new Map([
      ['email', new EmailValidator()],
      ['password', new PasswordValidator()],
      ['phone', new PhoneValidator()],
      ['libraryCard', new LibraryCardValidator()],
    ]);
  }

  validateField(fieldName: string, value: string): string | null {
    const validator = this.validators.get(fieldName);
    return validator ? validator.validate(value) : null;
  }

  validateForm(formData: Record<string, string>): Record<string, string | null> {
    const errors: Record<string, string | null> = {};

    for (const [fieldName, value] of Object.entries(formData)) {
      errors[fieldName] = this.validateField(fieldName, value);
    }

    return errors;
  }
}