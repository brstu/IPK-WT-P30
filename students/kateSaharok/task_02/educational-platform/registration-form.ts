import { FormData } from './form-types';
import { FormValidator } from './form-validators';

export class RegistrationForm {
  private form: HTMLFormElement;
  private fullNameInput: HTMLInputElement;
  private emailInput: HTMLInputElement;
  private passwordInput: HTMLInputElement;
  private phoneInput: HTMLInputElement;
  private successMessage: HTMLElement;

  constructor(private container: HTMLElement) {
    this.render();
    this.form = this.container.querySelector('#registrationForm') as HTMLFormElement;
    this.fullNameInput = this.container.querySelector('#fullName') as HTMLInputElement;
    this.emailInput = this.container.querySelector('#email') as HTMLInputElement;
    this.passwordInput = this.container.querySelector('#password') as HTMLInputElement;
    this.phoneInput = this.container.querySelector('#phone') as HTMLInputElement;
    this.successMessage = this.container.querySelector('#success-message') as HTMLElement;

    this.initializeEventListeners();
  }

  private render(): void {
    this.container.innerHTML = `
      <div class="container">
        <div class="header">
          <h1>Образовательная платформа</h1>
          <p>Регистрация нового пользователя</p>
        </div>
        
        <form id="registrationForm" class="registration-form" novalidate>
          <div class="form-group">
            <label for="fullName">ФИО</label>
            <input type="text" id="fullName" name="fullName" required aria-describedby="name-error" placeholder="Иванов Иван Иванович">
            <div id="name-error" class="error-message" aria-live="polite"></div>
          </div>

          <div class="form-group">
            <label for="email">Email</label>
            <input type="email" id="email" name="email" required aria-describedby="email-error" placeholder="your@email.com">
            <div id="email-error" class="error-message" aria-live="polite"></div>
          </div>

          <div class="form-group">
            <label for="password">Пароль</label>
            <input type="password" id="password" name="password" required aria-describedby="password-error" placeholder="Придумайте пароль">
            <div id="password-error" class="error-message" aria-live="polite"></div>
            <div class="password-requirements">
              Требования: минимум 1 специальный символ (@, #, $, %, &) и минимум 2 цифры
            </div>
          </div>

          <div class="form-group">
            <label for="phone">Телефон</label>
            <input type="tel" id="phone" name="phone" placeholder="+79991234567" required aria-describedby="phone-error">
            <div id="phone-error" class="error-message" aria-live="polite"></div>
          </div>

          <button type="submit" class="submit-btn">Зарегистрироваться</button>
          
          <div id="success-message" class="success-message">
            Регистрация успешно завершена!
          </div>
        </form>
      </div>
    `;
  }

  private initializeEventListeners(): void {
    this.form.addEventListener('submit', this.handleSubmit.bind(this));
    
    this.fullNameInput.addEventListener('blur', () => this.validateField('fullName'));
    this.emailInput.addEventListener('blur', () => this.validateField('email'));
    this.passwordInput.addEventListener('blur', () => this.validateField('password'));
    this.phoneInput.addEventListener('blur', () => this.validateField('phone'));
    this.phoneInput.addEventListener('input', this.formatPhone.bind(this));
  }

  private formatPhone(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');
    
    // Добавляем + если его нет
    if (value && !value.startsWith('+')) {
      value = '+' + value;
    }

    // Ограничиваем длину (1+15 цифр)
    if (value.length > 16) {
      value = value.slice(0, 16);
    }

    input.value = value;
  }

  private validateField(fieldName: string): void {
    const formData = this.getFormData();
    let error: string | null = null;

    switch (fieldName) {
      case 'fullName':
        error = FormValidator.validateFullName(formData.fullName);
        break;
      case 'email':
        error = FormValidator.validateEmail(formData.email);
        break;
      case 'password':
        error = FormValidator.validatePassword(formData.password);
        break;
      case 'phone':
        error = FormValidator.validatePhone(formData.phone);
        break;
      default:
        break;
    }

    this.showError(fieldName, error);
  }

  private getFormData(): FormData {
    return {
      fullName: this.fullNameInput.value.trim(),
      email: this.emailInput.value.trim(),
      password: this.passwordInput.value,
      phone: this.phoneInput.value.trim(),
    };
  }

  private showError(fieldName: string, error: string | null): void {
    const errorElement = document.getElementById(`${fieldName}-error`);
    const inputElement = document.getElementById(fieldName);
    
    if (errorElement && inputElement) {
      if (error) {
        errorElement.textContent = error;
        inputElement.setAttribute('aria-invalid', 'true');
      } else {
        errorElement.textContent = '';
        inputElement.setAttribute('aria-invalid', 'false');
      }
    }
  }

  private clearErrors(): void {
    const errorElements = this.container.querySelectorAll('.error-message');
    const inputElements = this.container.querySelectorAll('input[aria-invalid]');
    
    errorElements.forEach((el) => {
      el.textContent = '';
    });
    inputElements.forEach((el) => {
      el.setAttribute('aria-invalid', 'false');
    });
    this.successMessage.style.display = 'none';
  }

  private handleSubmit(event: Event): void {
    event.preventDefault();
    
    this.clearErrors();
    
    const formData = this.getFormData();
    const validationResult = FormValidator.validateForm(formData);

    if (!validationResult.isValid) {
      Object.entries(validationResult.errors).forEach(([field, error]) => {
        if (error) {
          this.showError(field, error);
        }
      });
      return;
    }

    this.showSuccess();
  }

  private showSuccess(): void {
    this.successMessage.style.display = 'block';
    setTimeout(() => {
      this.form.reset();
      this.successMessage.style.display = 'none';
    }, 3000);
  }
}