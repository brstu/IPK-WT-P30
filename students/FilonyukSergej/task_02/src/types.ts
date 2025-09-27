export interface FormData {
    email: string;
    phone: string;
    password: string;
    skills: string[];
}

export interface ValidationResult {
    isValid: boolean;
    errors: {
        email?: string;
        phone?: string;
        password?: string;
        skills?: string;
    };
}

export interface ValidationRules {
    email: (value: string) => string | null;
    phone: (value: string) => string | null;
    password: (value: string) => string | null;
    skills: (values: string[]) => string | null;
}