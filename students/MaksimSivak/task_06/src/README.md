Лабораторная работа 06. REST API с версионированием и валидацией
 Описание проекта
REST API для финансового учета с поддержкой версионирования (v1 и v2), валидацией данных и документацией Swagger.

 Запуск приложения
Предварительные требования
.NET 8.0 SDK или выше

PowerShell или командная строка

Шаги запуска:
Перейдите в папку проекта:

Восстановите зависимости:

bash
dotnet restore
Соберите проект:

bash
dotnet build
Запустите приложение:

bash
dotnet run
Откройте в браузере:

text
http://localhost:5150/swagger
📊 Структура API
Версия 1.0
GET /api/v1/expensecategories - Получить список категорий расходов

GET /api/v1/expensecategories/{id} - Получить категорию по ID

Версия 2.0
GET /api/v2/expensecategories - Получить список категорий (расширенный)

POST /api/v2/expensecategories - Создать новую категорию

GET /api/v2/expensecategories/{id} - Получить категорию по ID

DELETE /api/v2/expensecategories/{id} - Удалить категорию

🎯 Примеры запросов
Создание категории (POST)
http
POST /api/v2/expensecategories
Content-Type: application/json

{
  "name": "Продукты",
  "description": "Покупка продуктов питания",
  "monthlyBudget": 15000,
  "colorCode": "#ff5733"
}
Успешный ответ:
json
{
  "id": 1,
  "name": "Продукты",
  "description": "Покупка продуктов питания",
  "monthlyBudget": 15000,
  "colorCode": "#ff5733",
  "createdAt": "2024-01-15T10:30:00Z"
}
Получение категорий с пагинацией (GET)
http
GET /api/v1/expensecategories?page=1&pageSize=10&sort=name&filter=еда
Заголовки ответа:
text
X-Total-Count: 25
X-Page: 1
X-Page-Size: 10
⚠️ Примеры ошибок
Ошибка валидации (400 Bad Request)
http
POST /api/v2/expensecategories
Content-Type: application/json

{
  "name": "",
  "monthlyBudget": -100,
  "colorCode": "invalid"
}
Ответ с ошибками:
json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "errors": {
    "Name": ["Название категории обязательно"],
    "MonthlyBudget": ["Бюджет не может быть отрицательным"],
    "ColorCode": ["Неверный формат цвета (должен быть #RRGGBB)"]
  }
}
Категория не найдена (404 Not Found)
http
GET /api/v1/expensecategories/999
Ответ:
json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Категория с ID 999 не найдена"
}
🛠️ Технологии
ASP.NET Core 8.0 - Веб-фреймворк

Swagger/OpenAPI - Документация API

FluentValidation - Валидация данных

API Versioning - Поддержка версий

DataAnnotations - Валидация моделей

📁 Структура проекта
text
FinancialAccounting.API/
├── Controllers/
│   └── ExpenseCategoriesController.cs
├── DTOs/
│   ├── v1/
│   │   └── ExpenseCategoryDtoV1.cs
│   └── v2/
│       └── ExpenseCategoryDtoV2.cs
├── Models/
│   └── ExpenseCategory.cs
├── Validators/
│   └── ExpenseCategoryValidator.cs
├── Program.cs
└── FinancialAccounting.API.csproj
🔧 Особенности реализации
Версионирование через URL: api/v{version}/[controller]

Пагинация: Параметры page, pageSize, sort, filter

Валидация: DataAnnotations + FluentValidation

Обработка ошибок: ProblemDetails для 400/404/500

Swagger: Автоматическая документация для обеих версий

📝 Примечания
Приложение запускается на порту 5150

Данные хранятся в памяти (при перезапуске сбрасываются)

Swagger UI доступен по адресу /swagger

Поддерживается Content Negotiation

🎯 Критерии выполнения
✅ Контроллеры с атрибутом [ApiController]

✅ Версии v1 и v2 в маршрутах

✅ Пагинация/фильтр/сортировка

✅ Swagger документация

✅ Валидация DataAnnotations/FluentValidation

✅ Возврат ProblemDetails для ошибок

Лабораторная работа выполнена в рамках курса "Веб-технологии"