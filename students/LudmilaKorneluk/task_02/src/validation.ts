export interface FormData {
  email: string;
  password: string;
  phone: string;
  libraryCard: string;
}

export interface ValidationResult {
  isValid: boolean;
  errors: {
    email?: string;
    password?: string;
    phone?: string;
    libraryCard?: string;
  };
}

export interface FieldValidator {
  validate(value: string): string | null;
}