export interface FormData {
  fullName: string;
  email: string;
  password: string;
  phone: string;
}

export interface ValidationResult {
  isValid: boolean;
  errors: {
    fullName?: string;
    email?: string;
    password?: string;
    phone?: string;
  };
}

export interface FormErrors {
  fullName?: string;
  email?: string;
  password?: string;
  phone?: string;
}