# Лабораторная работа 07

## Скриншоты:

  (img\lp7_curl.png)
  (img\lp7_ETag.png)
  (img\lp7_ProblemDetails.png)
  (img\lp7_Tests.png)
  (img\lp7_Swagger.png)

## Выполненные задания

### 1. Тесты
- Создано 5+ юнит-тестов для контроллеров
- Проверены коды ответов (200, 404, 201)
- Проверена сериализация данных

### 2. Кэш и ETag
- Включен ResponseCaching
- Добавлена простая ETag логика
- Настроены заголовки Cache-Control

### 3. Обработка ошибок и устойчивость
- Глобальный обработчик исключений с ProblemDetails
- Correlation ID через TraceIdentifier
- Polly retry политика (3 попытки с экспоненциальной задержкой)
- Polly timeout политика (10 секунд)

### 4. Оптимизация
- Включено сжатие ответов (Gzip, Brotli)
- Настроен System.Text.Json (CamelCase, IgnoreNullValues)
- Response compression для JSON, XML, текстов

### 5. End-to-End сценарий
- Создание продукта → получение внешних данных → обновление → кэширование
- Полная цепочка с обработкой ошибок и retry механизмом

## Примеры

### ProblemDetails пример:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal Server Error",
  "status": 500,
  "detail": "An unexpected error occurred",
  "instance": "/api/products",
  "correlationId": "0HN0VJ9U2D9J1:00000001"
}
        