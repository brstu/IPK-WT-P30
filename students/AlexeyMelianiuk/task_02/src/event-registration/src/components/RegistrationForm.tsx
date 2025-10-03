import React, { useState, useRef, useEffect } from 'react';
import {
  RegistrationFormData,
  ValidationErrors,
  validateForm,
  validateEmail,
  validatePassword,
  validatePhone,
  validateInterests,
} from '../utils/validation';
import '../styles/form.css';

const RegistrationForm: React.FC = () => {
  // Состояния формы
  const [formData, setFormData] = useState<RegistrationFormData>({
    email: '',
    password: '',
    phone: '',
    interests: '',
  });

  const [errors, setErrors] = useState<ValidationErrors>({});
  const [touched, setTouched] = useState<Partial<Record<keyof RegistrationFormData, boolean>>>({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);

  // Refs для доступности
  const liveRegionRef = useRef<HTMLDivElement>(null);
  const formRef = useRef<HTMLFormElement>(null);

  // Обновление live region для скринридеров
  useEffect(() => {
    if (liveRegionRef.current) {
      const errorMessages = Object.values(errors).filter(Boolean);
      if (errorMessages.length > 0) {
        liveRegionRef.current.textContent = `Ошибки формы: ${errorMessages.join(', ')}`;
      } else if (isSuccess) {
        liveRegionRef.current.textContent = 'Регистрация успешно завершена';
      } else {
        liveRegionRef.current.textContent = '';
      }
    }
  }, [errors, isSuccess]);

  // Обработчики изменений
  const handleInputChange = (field: keyof RegistrationFormData) => (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const { value } = event.target;
    setFormData(prev => ({ ...prev, [field]: value }));

    // Валидация при изменении (только для touched полей)
    if (touched[field]) {
      validateField(field, value);
    }
  };

  const handleBlur = (field: keyof RegistrationFormData) => () => {
    setTouched(prev => ({ ...prev, [field]: true }));
    validateField(field, formData[field]);
  };

  // Валидация отдельного поля
  const validateField = (field: keyof RegistrationFormData, value: string): void => {
    let error: string | undefined;

    switch (field) {
      case 'email':
        error = validateEmail(value);
        break;
      case 'password':
        error = validatePassword(value);
        break;
      case 'phone':
        error = validatePhone(value);
        break;
      case 'interests':
        error = validateInterests(value);
        break;
    }

    setErrors(prev => ({
      ...prev,
      [field]: error,
    }));
  };

  // Обработчик отправки формы
  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    
    // Помечаем все поля как touched
    const allTouched = {
      email: true,
      password: true,
      phone: true,
      interests: true,
    };
    setTouched(allTouched);

    // Валидация всей формы
    const formErrors = validateForm(formData);
    setErrors(formErrors);

    // Проверяем есть ли ошибки
    if (Object.keys(formErrors).length === 0) {
      setIsSubmitting(true);
      
      // Имитация отправки на сервер
      try {
        await new Promise(resolve => setTimeout(resolve, 1000));
        setIsSuccess(true);
        setFormData({ email: '', password: '', phone: '', interests: '' });
        setTouched({});
        
        // Фокус на форме для скринридеров
        formRef.current?.focus();
      } catch (error) {
        setErrors({ submit: 'Ошибка при отправке формы. Попробуйте еще раз.' });
      } finally {
        setIsSubmitting(false);
      }
    }
  };

  // Проверка валидности формы
  const isFormValid = Object.keys(errors).length === 0;

  if (isSuccess) {
    return (
      <div className="registration-form" role="main">
        <div 
          className="success-message" 
          role="alert"
          aria-live="polite"
        >
          <h2>Регистрация успешно завершена!</h2>
          <p>Мы отправили подтверждение на вашу электронную почту.</p>
          <button
            type="button"
            className="submit-button"
            onClick={() => setIsSuccess(false)}
          >
            Зарегистрировать еще одного участника
          </button>
        </div>
      </div>
    );
  }

  return (
    <form
      ref={formRef}
      className="registration-form"
      onSubmit={handleSubmit}
      noValidate
      aria-labelledby="form-title"
    >
      {/* Live region для скринридеров */}
      <div
        ref={liveRegionRef}
        className="aria-live"
        aria-live="polite"
        aria-atomic="true"
      />

      <h1 id="form-title" className="form-title">
        Регистрация на мероприятие
      </h1>

      {/* Поле Email */}
      <div className="form-group">
        <label htmlFor="email" className="form-label">
          Email адрес *
        </label>
        <input
          id="email"
          type="email"
          className={`form-input ${touched.email && errors.email ? 'error' : ''}`}
          value={formData.email}
          onChange={handleInputChange('email')}
          onBlur={handleBlur('email')}
          required
          aria-required="true"
          aria-describedby={touched.email && errors.email ? 'email-error' : 'email-hint'}
          aria-invalid={touched.email && !!errors.email}
        />
        {touched.email && errors.email && (
          <div id="email-error" className="error-message" role="alert">
            {errors.email}
          </div>
        )}
        <div id="email-hint" className="form-hint">
          Введите действующий email адрес
        </div>
      </div>

      {/* Поле Пароль */}
      <div className="form-group">
        <label htmlFor="password" className="form-label">
          Пароль *
        </label>
        <input
          id="password"
          type="password"
          className={`form-input ${touched.password && errors.password ? 'error' : ''}`}
          value={formData.password}
          onChange={handleInputChange('password')}
          onBlur={handleBlur('password')}
          required
          aria-required="true"
          aria-describedby={touched.password && errors.password ? 'password-error' : 'password-hint'}
          aria-invalid={touched.password && !!errors.password}
        />
        {touched.password && errors.password && (
          <div id="password-error" className="error-message" role="alert">
            {errors.password}
          </div>
        )}
        <div id="password-hint" className="form-hint">
          Минимум 8 символов, без повторения одного символа 3+ раз подряд
        </div>
      </div>

      {/* Поле Телефон */}
      <div className="form-group">
        <label htmlFor="phone" className="form-label">
          Телефон *
        </label>
        <input
          id="phone"
          type="tel"
          className={`form-input ${touched.phone && errors.phone ? 'error' : ''}`}
          value={formData.phone}
          onChange={handleInputChange('phone')}
          onBlur={handleBlur('phone')}
          required
          aria-required="true"
          aria-describedby={touched.phone && errors.phone ? 'phone-error' : 'phone-hint'}
          aria-invalid={touched.phone && !!errors.phone}
        />
        {touched.phone && errors.phone && (
          <div id="phone-error" className="error-message" role="alert">
            {errors.phone}
          </div>
        )}
        <div id="phone-hint" className="form-hint">
          Формат: +7 123 456-78-90 (минимум 10 цифр)
        </div>
      </div>

      {/* Поле Интересы */}
      <div className="form-group">
        <label htmlFor="interests" className="form-label">
          Теги интересов *
        </label>
        <input
          id="interests"
          type="text"
          className={`form-input ${touched.interests && errors.interests ? 'error' : ''}`}
          value={formData.interests}
          onChange={handleInputChange('interests')}
          onBlur={handleBlur('interests')}
          required
          aria-required="true"
          aria-describedby={touched.interests && errors.interests ? 'interests-error' : 'interests-hint'}
          aria-invalid={touched.interests && !!errors.interests}
          placeholder="programming, design, marketing"
        />
        {touched.interests && errors.interests && (
          <div id="interests-error" className="error-message" role="alert">
            {errors.interests}
          </div>
        )}
        <div id="interests-hint" className="form-hint">
          Перечислите интересы через запятую (только латинские буквы, 2-20 символов каждый)
        </div>
      </div>

      {/* Кнопка отправки */}
      <button
        type="submit"
        className="submit-button"
        disabled={isSubmitting || (!isFormValid && Object.keys(touched).length > 0)}
        aria-describedby={errors.submit ? 'submit-error' : undefined}
      >
        {isSubmitting ? 'Регистрация...' : 'Зарегистрироваться'}
      </button>

      {errors.submit && (
        <div id="submit-error" className="error-message" role="alert">
          {errors.submit}
        </div>
      )}
    </form>
  );
};

export default RegistrationForm;