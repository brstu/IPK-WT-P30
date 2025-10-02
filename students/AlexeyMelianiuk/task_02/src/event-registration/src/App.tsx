import React from 'react';
import RegistrationForm from './components/RegistrationForm';
import './styles/form.css';

const App: React.FC = () => {
  return (
    <div className="app">
      <header role="banner">
        <h1>Регистрация на митап</h1>
      </header>
      <main>
        <RegistrationForm />
      </main>
    </div>
  );
};

export default App;