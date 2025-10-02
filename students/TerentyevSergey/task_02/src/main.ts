// Простые функции валидации
function validateEmail(email: string): string {
  if (!email) return 'Email обязателен';
  if (!email.includes('@')) return 'Email должен содержать @';
  if (!email.includes('.')) return 'Email должен содержать точку';
  return '';
}

function validatePhone(phone: string): string {
  if (!phone) return 'Телефон обязателен';
  if (!phone.startsWith('+')) return 'Телефон должен начинаться с +';
  if (!/^\+\d+$/.test(phone)) return 'Телефон должен содержать только цифры после +';
  if (phone.length < 11) return 'Телефон слишком короткий';
  if (phone.length > 16) return 'Телефон слишком длинный';
  return '';
}

function validatePassword(password: string): string {
  if (!password) return 'Пароль обязателен';
  if (password.length < 8) return 'Пароль должен быть минимум 8 символов';
  if (!/[a-zA-Z]/.test(password)) return 'Добавьте буквы в пароль';
  if (!/\d/.test(password)) return 'Добавьте цифры в пароль';
  if (/(.{2,})\1{2,}/.test(password)) return 'Слишком много повторов в пароле';
  return '';
}

function validatePromoCode(promoCode: string): string {
  if (!promoCode) return '';
  if (!/^[A-Z]{3}-\d{3}-[A-Z]{3}$/.test(promoCode)) {
    return 'Формат: ABC-123-DEF (большие буквы)';
  }
  return '';
}

// Главная функция - запускается при загрузке страницы
function initForm() {
  console.log('Форма загружается...');
  
  const form = document.getElementById('registrationForm') as HTMLFormElement;
  const submitBtn = document.getElementById('submitBtn') as HTMLButtonElement;
  const emailInput = document.getElementById('email') as HTMLInputElement;
  const phoneInput = document.getElementById('phone') as HTMLInputElement;
  const passwordInput = document.getElementById('password') as HTMLInputElement;
  const promoInput = document.getElementById('promoCode') as HTMLInputElement;

  // Функция показа ошибки
  function showError(fieldName: string, message: string) {
    const errorElement = document.getElementById(fieldName + 'Error');
    if (errorElement) {
      errorElement.textContent = message;
      errorElement.style.color = 'red';
      errorElement.style.fontSize = '14px';
    }
  }

  // Функция проверки одного поля
  function checkField(input: HTMLInputElement) {
    const value = input.value;
    let error = '';

    switch (input.id) {
      case 'email': error = validateEmail(value); break;
      case 'phone': error = validatePhone(value); break;
      case 'password': error = validatePassword(value); break;
      case 'promoCode': error = validatePromoCode(value); break;
    }

    showError(input.id + 'Error', error);
    updateSubmitButton();
  }

  // Функция обновления кнопки
  function updateSubmitButton() {
    const emailError = validateEmail(emailInput.value);
    const phoneError = validatePhone(phoneInput.value);
    const passwordError = validatePassword(passwordInput.value);
    const promoError = validatePromoCode(promoInput.value);

    const hasErrors = emailError || phoneError || passwordError || promoError;
    const allFilled = emailInput.value && phoneInput.value && passwordInput.value;

    submitBtn.disabled = !!hasErrors || !allFilled;
  }

  // Вешаем обработчики событий
  emailInput.addEventListener('input', () => checkField(emailInput));
  phoneInput.addEventListener('input', () => checkField(phoneInput));
  passwordInput.addEventListener('input', () => checkField(passwordInput));
  promoInput.addEventListener('input', () => checkField(promoInput));

  emailInput.addEventListener('blur', () => checkField(emailInput));
  phoneInput.addEventListener('blur', () => checkField(phoneInput));
  passwordInput.addEventListener('blur', () => checkField(passwordInput));
  promoInput.addEventListener('blur', () => checkField(promoInput));

  // Обработчик отправки формы
  form.addEventListener('submit', (e) => {
    e.preventDefault();
    
    // Финальная проверка
    checkField(emailInput);
    checkField(phoneInput);
    checkField(passwordInput);
    checkField(promoInput);

    const emailError = validateEmail(emailInput.value);
    const phoneError = validatePhone(phoneInput.value);
    const passwordError = validatePassword(passwordInput.value);
    const promoError = validatePromoCode(promoInput.value);

    if (!emailError && !phoneError && !passwordError && !promoError) {
      alert('✅ Регистрация успешна!');
      console.log('Данные:', {
        email: emailInput.value,
        phone: phoneInput.value,
        password: passwordInput.value,
        promoCode: promoInput.value
      });
      form.reset();
      updateSubmitButton();
    } else {
      alert('❌ Исправьте ошибки!');
    }
  });

  // Изначально блокируем кнопку
  updateSubmitButton();
  
  console.log('Форма готова!');
}

// Запускаем когда страница загрузится
document.addEventListener('DOMContentLoaded', initForm);