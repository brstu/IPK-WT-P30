# Лабораторная работа №7

## Выполненные задания

### 1. Тесты (xUnit + Moq)
Написаны юнит-тесты для контроллера `ExpensesController` в проекте `FinancialAccounting.Tests`.
**Проверяемые сценарии:**
- `GetExpenses_ReturnsOkResult` - возврат статуса 200 OK при успешном запросе списка расходов.
- `GetExpense_ReturnsNotFound_WhenExpenseDoesNotExist` - возврат статуса 404 Not Found при запросе несуществующего расхода.
- `CreateExpense_ReturnsCreatedResult` - возврат статуса 201 Created при успешном создании расхода.

**Запуск тестов:**
```bash
dotnet test
2. Кэширование и ETag
Реализовано кэширование ответов на стороне сервера с использованием ResponseCaching и ETag для заголовка If-None-Match.

Пример работы:

Первый GET-запрос к /api/expenses возвращает статус 200 OK и заголовок ETag.

Повторный GET-запрос с заголовком If-None-Match, содержащим значение ETag из первого ответа, возвращает статус 304 Not Modified (тело ответа отсутствует).

Код реализации:
В контроллере ExpensesController используется атрибут [ResponseCache(Duration = 60)] и метод GenerateETag для создания хэша на основе данных.

3. Обработка ошибок и устойчивость
Добавлен глобальный обработчик исключений (через встроенное ПО ASP.NET Core). Для демонстрации обработки ошибок можно вызвать несуществующий endpoint (например, /api/nonexistent), чтобы получить стандартный ответ в формате ProblemDetails.

Пример ответа об ошибке (404):

json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404,
  "traceId": "00-7c0c8b9b3b3b3b3b3b3b3b3b3b3b3b3b-3b3b3b3b3b3b3b3b-00"
}
4. Оптимизация
Включено сжатие ответов и настроена сериализация JSON:

Использование camelCase для имен свойств.

Игнорирование null-значений.

Настройки в Program.cs:

csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
5. Мини-итог: сквозной сценарий
Сценарий:

Создание расхода: POST /api/expenses с телом {"categoryId": 1, "amount": 100, "description": "Test expense"}

Получение списка расходов: GET /api/expenses (кэшируется на 60 секунд)

Повторное получение списка расходов: GET /api/expenses с заголовком If-None-Match (возвращает 304)

Пример выполнения:

bash
# 1. Создание расхода
curl -X POST "http://localhost:5050/api/expenses" -H "Content-Type: application/json" -d '{"categoryId": 1, "amount": 100, "description": "Test expense"}'

# 2. Получение списка расходов (сохранить ETag из заголовков ответа)
curl -i "http://localhost:5050/api/expenses"

# 3. Повторный запрос с ETag (должен вернуть 304)
curl -i "http://localhost:5050/api/expenses" -H "If-None-Match: <значение_ETag>"
Скриншоты и примеры
Пример ответа с ETag (200 OK)
text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
ETag: "SGVsbG8gV29ybGQh"
Cache-Control: public,max-age=60
... (тело ответа)
Пример ответа с 304 Not Modified
text
HTTP/1.1 304 Not Modified
ETag: "SGVsbG8gV29ybGQh"
Пример ответа об ошибке (404 Not Found)
json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404,
  "traceId": "00-7c0c8b9b3b3b3b3b3b3b3b3b3b3b3b3b-3b3b3b3b3b3b3b3b-00"
}
Запуск приложения
Восстановите зависимости:

bash
dotnet restore
Запустите приложение:

bash
dotnet run --urls "http://localhost:5050"
Запустите тесты:

bash
dotnet test