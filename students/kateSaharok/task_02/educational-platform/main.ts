import './style.css';
import { RegistrationForm } from './registration-form';

const app = document.querySelector<HTMLDivElement>('#app')!;

new RegistrationForm(app);