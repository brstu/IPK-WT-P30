import { FormData, ValidationResult } from './types';
import { validateForm, validationRules } from './validation';

class RegistrationForm {
    private form: HTMLFormElement | null = null;
    private emailInput: HTMLInputElement | null = null;
    private phoneInput: HTMLInputElement | null = null;
    private passwordInput: HTMLInputElement | null = null;
    private skillsCheckboxes: NodeListOf<HTMLInputElement> | null = null;
    private successMessage: HTMLElement | null = null;

    constructor() {
        this.initializeElements();
        if (this.isFormReady()) {
            this.setupEventListeners();
        } else {
            console.error('Не все элементы формы найдены в DOM');
        }
    }

    private initializeElements(): void {
        this.form = document.getElementById('registrationForm') as HTMLFormElement | null;
        this.emailInput = document.getElementById('email') as HTMLInputElement | null;
        this.phoneInput = document.getElementById('phone') as HTMLInputElement | null;
        this.passwordInput = document.getElementById('password') as HTMLInputElement | null;
        this.skillsCheckboxes = document.querySelectorAll('input[name="skills"]');
        this.successMessage = document.getElementById('success-message') as HTMLElement | null;
    }

    private isFormReady(): boolean {
        return !!(
            this.form &&
            this.emailInput &&
            this.phoneInput &&
            this.passwordInput &&
            this.skillsCheckboxes &&
            this.skillsCheckboxes.length > 0 &&
            this.successMessage
        );
    }

    private setupEventListeners(): void {
        if (!this.form || !this.emailInput || !this.phoneInput || !this.passwordInput || !this.skillsCheckboxes) {
            return;
        }

        this.form.addEventListener('submit', this.handleSubmit.bind(this));
        
        // Простые обработчики blur
        this.emailInput.addEventListener('blur', () => {
            this.validateEmail();
        });
        
        this.phoneInput.addEventListener('blur', () => {
            this.validatePhone();
        });
        
        this.passwordInput.addEventListener('blur', () => {
            this.validatePassword();
        });
        
        this.phoneInput.addEventListener('input', this.handlePhoneInput.bind(this));
        
        this.skillsCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', () => this.validateSkills());
        });
    }

    private handleSubmit(event: Event): void {
        event.preventDefault();
        
        const formData = this.getFormData();
        const validationResult = validateForm(formData);

        this.displayValidationErrors(validationResult);

        if (validationResult.isValid) {
            this.showSuccess();
            this.form?.reset();
            this.clearAllErrors();
        }
    }

    private getFormData(): FormData {
        const selectedSkills = Array.from(this.skillsCheckboxes || [])
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);

        return {
            email: this.emailInput?.value.trim() || '',
            phone: this.phoneInput?.value.trim() || '',
            password: this.passwordInput?.value || '',
            skills: selectedSkills
        };
    }

 private validateEmail(): void {
    const email = this.emailInput?.value.trim() || '';
    const error = validationRules.email(email);
    this.displayError('email', error || undefined);
}

private validatePhone(): void {
    const phone = this.phoneInput?.value.trim() || '';
    const error = validationRules.phone(phone);
    this.displayError('phone', error || undefined);
}

private validatePassword(): void {
    const password = this.passwordInput?.value || '';
    const error = validationRules.password(password);
    this.displayError('password', error || undefined);
}

private validateSkills(): void {
    const selectedSkills = Array.from(this.skillsCheckboxes || [])
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value);
    
    const error = validationRules.skills(selectedSkills);
    this.displayError('skills', error || undefined);
}

    private displayValidationErrors(validationResult: ValidationResult): void {
        this.displayError('email', validationResult.errors.email);
        this.displayError('phone', validationResult.errors.phone);
        this.displayError('password', validationResult.errors.password);
        this.displayError('skills', validationResult.errors.skills);
    }

    private displayError(field: keyof FormData, error?: string): void {
        const errorElement = document.getElementById(`${field}-error`);
        
        if (errorElement) {
            errorElement.textContent = error || '';
            errorElement.style.display = error ? 'block' : 'none';
            
            const inputElement = this.getInputElement(field);
            if (inputElement) {
                if (error) {
                    inputElement.classList.add('error');
                    inputElement.setAttribute('aria-invalid', 'true');
                } else {
                    inputElement.classList.remove('error');
                    inputElement.removeAttribute('aria-invalid');
                }
            }
        }
    }

    private clearAllErrors(): void {
        const errorElements = document.querySelectorAll('.error-message');
        errorElements.forEach(element => {
            (element as HTMLElement).textContent = '';
            (element as HTMLElement).style.display = 'none';
        });

        const inputElements = document.querySelectorAll('input');
        inputElements.forEach(input => {
            input.classList.remove('error');
            input.removeAttribute('aria-invalid');
        });
    }

    private getInputElement(field: keyof FormData): HTMLElement | null {
        switch (field) {
            case 'email':
                return this.emailInput;
            case 'phone':
                return this.phoneInput;
            case 'password':
                return this.passwordInput;
            default:
                return null;
        }
    }

    private handlePhoneInput(event: Event): void {
        const input = event.target as HTMLInputElement;
        let value = input.value.replace(/\D/g, '');
        
        if (value.startsWith('375')) {
            value = '+' + value;
        } else if (value.length > 0 && !value.startsWith('+')) {
            value = '+375' + value;
        }
        
        // Форматирование номера
        if (value.length > 4) {
            value = value.replace(/(\+\d{3})(\d{2})(\d{3})(\d{2})(\d{2})/, '$1 $2 $3-$4-$5');
        }
        
        // Убедимся, что не превышаем максимальную длину
        if (value.length > 17) {
            value = value.substring(0, 17);
        }
        
        input.value = value;
    }

    private showSuccess(): void {
        if (this.successMessage) {
            this.successMessage.hidden = false;
            
            setTimeout(() => {
                if (this.successMessage) {
                    this.successMessage.hidden = true;
                }
            }, 3000);
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new RegistrationForm();
});