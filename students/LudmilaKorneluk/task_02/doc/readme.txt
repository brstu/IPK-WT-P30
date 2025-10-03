Лабораторная работа 02 - Библиотека
ФИО: Шаховец Лидия Андреевна
Идентификатор: LidyaShakhovets
Вариант: 14 

Структура проекта
project/
├── src/
│   ├── types/
│   │   └── validation.ts
│   ├── utils/
│   │   └── validators.ts
│   ├── styles/
│   │   └── main.css
│   ├── main.ts
│   └── index.html
├── package.json
├── tsconfig.json
├── vite.config.ts
├── .eslintrc.js
└── .prettierrc


Запуск проекта
Установите зависимости:

bash
npm install
Запустите в режиме разработки:

bash
npm run dev
Проверьте линтинг:

bash
npm run lint
Соберите проект:

bash
npm run build
Тестовые данные для проверки
Валидные данные:

Email: test@example.com

Пароль: MySecure123

Телефон: +79161234567 или +375291234567

Читательский билет: LC‑2025‑12345

Невалидные данные для проверки ошибок:

Email: invalid-email

Пароль: myPassword123 (содержит "password")

Телефон: 123456 (некорректный формат)

Читательский билет: LC-2025-123 (неправильный формат)