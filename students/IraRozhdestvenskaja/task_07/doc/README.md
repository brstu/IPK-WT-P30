# Лабораторная работа 07

## REST API версии, Swagger, валидация

**Выполнила:** [Бортюк И.А.]  
**Группа:** [П-30] 

## Цель работы

*Освоить создание производительных, отказоустойчивых и хорошо тестируемых Web API с использованием современных практик разработки.

## Выполненные задачи

### Тесты API 
Реализованные тесты
- GetItems_Returns200 - проверка успешного получения списка

- GetItem_ExistingId_Returns200 - проверка получения существующего элемента

- GetItem_NonExistingId_Returns404 - проверка обработки несуществующего элемента

- CreateItem_ValidData_Returns201 - проверка создания элемента

- GetItems_ReturnsCamelCaseJson - проверка формата JSON

Результаты тестирования

bash
dotnet test

### Кэш и ETag 
*Реализация
Добавлен ResponseCaching middleware в Program.cs

Реализовано вычисление ETag на основе SHA256 хеша содержимого

Настроены заголовки Cache-Control

*Пример работы ETag

-Первый запрос (200 OK):

http
GET /api/items/1
Response: 200 OK
ETag: "LXZWOY+sjJCMYPluxHHs3JGHdSjNZNChKw/OCKbpWKo="
Cache-Control: public, max-age=30

-Второй запрос (304 Not Modified):

http
GET /api/items/1
If-None-Match: "LXZWOY+sjJCMYPluxHHs3JGHdSjNZNChKw/OCKbpWKo="
Response: 304 Not Modified

### Обработка ошибок 
*Глобальный обработчик ошибок
Реализован middleware для обработки исключений с возвратом стандартизированных ошибок в формате ProblemDetails.

-Примеры ошибок

404 Not Found
json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Item with id 999 not found",
  "instance": "/api/items/999"
}

500 Internal Server Error
json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal Server Error",
  "status": 500,
  "detail": "This is a test exception for error handling",
  "instance": "/api/items/error",
  "traceId": "0HMV9V6R9JQ3S:00000001"
}

400 Bad Request
json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Item name is required",
  "instance": "/api/items"
}

### Оптимизация 
*Реализованные оптимизации

-Сжатие ответов
csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

-Настройка JSON сериализации
csharp
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

-Устойчивость (Polly)
csharp
builder.Services.AddHttpClient("ExternalService")
    .AddTransientHttpErrorPolicy(policy => 
        policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));


## Структура проекта

Task07/
├── Task07.Api/
│   ├── Controllers/
│   │   └── ItemsController.cs
│   ├── Program.cs
│   └── Properties/
│       └── launchSettings.json
├── Task07.Tests/
│   └── ItemsControllerTests.cs
└── Task07.sln



## Инструкция по запуску

-Перейти в папку проекта

\ЛP_7\src\Task07.Api

- Собираем проект

dotnet build

-Запускаем

dotnet run

## В новом окне PowerShell :

-Запуск тестов

dotnet test

-Запуск API

dotnet run --project Task07.Api

-Ручная проверка функциональности

Проверка кэширования

// Получить ETag

curl -v http://localhost:5000/api/items/1

// Проверить 304

curl -v -H "If-None-Match: \"ETAG_VALUE\"" http://localhost:5000/api/items/1

-Проверка ошибок

// 404
curl http://localhost:5000/api/items/999

// 500
curl http://localhost:5000/api/items/error

// Сжатие
curl -H "Accept-Encoding: gzip" http://localhost:5000/api/items -v

Проверка в PowerShell

// Успешный запрос
Invoke-RestMethod -Uri "http://localhost:5000/api/items/1" -Method Get

// Проверка ETag
$response1 = Invoke-WebRequest -Uri "http://localhost:5000/api/items/1" -Method Get
$etag = $response1.Headers['ETag']
$headers = @{ "If-None-Match" = $etag }
Invoke-WebRequest -Uri "http://localhost:5000/api/items/1" -Method Get -Headers $headers