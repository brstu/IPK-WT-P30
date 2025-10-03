import { FormData, ValidationResult } from './types/validation';
import { FormValidator } from './utils/validators';

class RegistrationForm {
  private form: HTMLFormElement;
  private emailInput: HTMLInputElement;
  private passwordInput: HTMLInputElement;
  private phoneInput: HTMLInputElement;
  private libraryCardInput: HTMLInputElement;
  private submitButton: HTMLButtonElement;
  
  private validator: FormValidator;

  constructor() {
    this.validator = new FormValidator();
    this.initializeElements();
    this.attachEventListeners();
  }

  private initializeElements(): void {
    this.form = document.getElementById('registrationForm') as HTMLFormElement;
    this.emailInput = document.getElementById('email') as HTMLInputElement;
    this.passwordInput = document.getElementById('password') as HTMLInputElement;
    this.phoneInput = document.getElementById('phone') as HTMLInputElement;
    this.libraryCardInput = document.getElementById('libraryCard') as HTMLInputElement;
    this.submitButton = document.getElementById('submitBtn') as HTMLButtonElement;
  }

  private attachEventListeners(): void {
    this.form.addEventListener('submit', this.handleSubmit.bind(this));
    
    // Валидация при вводе
    [this.emailInput, this.passwordInput, this.phoneInput, this.libraryCardInput].forEach(input => {
      input.addEventListener('blur', this.validateField.bind(this));
      input.addEventListener('input', this.clearFieldError.bind(this));
    });
  }

  private validateField(event: Event): void {
    const target = event.target as HTMLInputElement;
    const fieldName = target.name;
    const value = target.value;

    const error = this.validator.validateField(fieldName, value);
    this.displayFieldError(fieldName, error);
  }

  private clearFieldError(event: Event): void {
    const target = event.target as HTMLInputElement;
    const fieldName = target.name;
    this.displayFieldError(fieldName, null);
  }

  private displayFieldError(fieldName: string, error: string | null): void {
    const errorElement = document.getElementById(`${fieldName}Error`);
    const inputElement = document.getElementById(fieldName) as HTMLInputElement;

    if (errorElement && inputElement) {
      if (error) {
        errorElement.textContent = error;
        errorElement.classList.add('visible');
        inputElement.classList.add('error');
      } else {
        errorElement.textContent = '';
        errorElement.classList.remove('visible');
        inputElement.classList.remove('error');
      }
    }
  }

  private getFormData(): FormData {
    return {
      email: this.emailInput.value,
      password: this.passwordInput.value,
      phone: this.phoneInput.value,
      libraryCard: this.libraryCardInput.value,
    };
  }

  private handleSubmit(event: Event): void {
    event.preventDefault();
    
    const formData = this.getFormData();
    const errors = this.validator.validateForm(formData);

    // Показать все ошибки
    Object.entries(errors).forEach(([fieldName, error]) => {
      this.displayFieldError(fieldName, error);
    });

    // Проверить, есть ли ошибки
    const hasErrors = Object.values(errors).some(error => error !== null);
    
    if (!hasErrors) {
      this.submitForm(formData);
    }
  }

  private submitForm(formData: FormData): void {
    this.submitButton.disabled = true;
    this.submitButton.textContent = 'Регистрация...';

    // Имитация отправки на сервер
    setTimeout(() => {
      console.log('Форма отправлена:', formData);
      alert('Регистрация успешно завершена!');
      this.form.reset();
      this.submitButton.disabled = false;
      this.submitButton.textContent = 'Зарегистрироваться';
    }, 1000);
  }
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
  new RegistrationForm();
});