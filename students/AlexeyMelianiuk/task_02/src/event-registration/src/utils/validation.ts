// Типы для формы
export interface RegistrationFormData {
  email: string;
  password: string;
  phone: string;
  interests: string;
}

export interface ValidationErrors {
  email?: string;
  password?: string;
  phone?: string;
  interests?: string;
}

// Регулярные выражения для валидации
const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

// Для телефона: допускаем +, пробелы, дефисы, затем проверяем что цифр >= 10
const PHONE_CLEAN_REGEX = /[^\d+]/g;
const PHONE_MIN_DIGITS = 10;

// Для пароля: проверка на повторение символа 3+ раз подряд
const PASSWORD_REPEAT_REGEX = /(.)\1\1/;

// Для тегов: латиница 2-20 символов, разделенные запятыми
const INTEREST_TAG_REGEX = /^[a-zA-Z]{2,20}$/;
const INTEREST_SEPARATOR_REGEX = /\s*,\s*/;

// Валидация email
export const validateEmail = (email: string): string | undefined => {
  if (!email.trim()) {
    return 'Email обязателен для заполнения';
  }
  
  if (!EMAIL_REGEX.
test(email)) {
    return 'Введите корректный email адрес';
  }
  
  return undefined;
};

// Валидация пароля
export const validatePassword = (password: string): string | undefined => {
  if (!password) {
    return 'Пароль обязателен для заполнения';
  }
  
  if (password.length < 8) {
    return 'Пароль должен содержать минимум 8 символов';
  }
  
  if (PASSWORD_REPEAT_REGEX.test(password)) {
    return 'Пароль не должен содержать один и тот же символ 3 раза подряд';
  }
  
  return undefined;
};

// Валидация телефона
export const validatePhone = (phone: string): string | undefined => {
  if (!phone.trim()) {
    return 'Телефон обязателен для заполнения';
  }
  
  // Очищаем от всего кроме цифр и плюса
  const cleanPhone = phone.replace(PHONE_CLEAN_REGEX, '');
  
  // Проверяем что плюс только в начале если есть
  if (cleanPhone.includes('+') && !cleanPhone.startsWith('+')) {
    return 'Знак + может быть только в начале номера';
  }
  
  // Убираем плюс для подсчета цифр
  const digitsOnly = cleanPhone.replace('+', '');
  
  if (digitsOnly.length < PHONE_MIN_DIGITS) {
    return `Телефон должен содержать минимум ${PHONE_MIN_DIGITS} цифр`;
  }
  
  return undefined;
};

// Валидация тегов интересов
export const validateInterests = (interests: string): string | undefined => {
  if (!interests.trim()) {
    return 'Укажите хотя бы один интерес';
  }
  
  const tags = interests.split(INTEREST_SEPARATOR_REGEX).filter(tag => tag.length > 0);
  
  if (tags.length === 0) {
    return 'Укажите хотя бы один интерес';
  }
  
  for (const tag of tags) {
    if (!INTEREST_TAG_REGEX.test(tag)) {
      return `Тег "${tag}" должен содержать только латинские буквы (2-20 символов)`;
    }
  }
  
  // Проверяем уникальность тегов
  const uniqueTags = new Set(tags.map(tag => tag.toLowerCase()));
  if (uniqueTags.size !== tags.length) {
    return 'Теги интересов не должны повторяться';
  }
  
  return undefined;
};

// Полная валидация формы
export const validateForm = (formData: RegistrationFormData): ValidationErrors => {
  const errors: ValidationErrors = {};
  
  const emailError = validateEmail(formData.email);
  const passwordError = validatePassword(formData.password);
  const phoneError = validatePhone(formData.phone);
  const interestsError = validateInterests(formData.interests);
  
  if (emailError) errors.email = emailError;
  if (passwordError) errors.password = passwordError;
  if (phoneError) errors.phone = phoneError;
  if (interestsError) errors.interests = interestsError;
  
  return errors;
};