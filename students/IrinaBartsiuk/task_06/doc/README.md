# Лабораторная работа 06

## REST API версии, Swagger, валидация

**Выполнила:** [Бортюк И.А.]  
**Группа:** [П-30] 

## Цель работы

Освоить принципы версионирования REST API, документирование с помощью Swagger и валидацию данных.

## Выполненные задачи

### Версионирование API

- Подключены пакеты для версионирования

- Настроено версионирование через URL сегмент

- Реализованы отдельные DTO модели для v1 и v2

- Маршруты: api/v1/products и api/v2/products

### Swagger документация

- Настроена генерация документации для разных версий API

- Добавлены описания и примеры для всех endpoints

- Документированы модели данных с аннотациями

### Валидация данных

- DataAnnotations для базовой валидации

- FluentValidation для сложной бизнес-логики

- Стандартизированные ответы об ошибках

### Пагинация и фильтрация

- Параметры: page, pageSize, sortBy, sortOrder, filter, category

- Поддержка в обеих версиях API

- Стандартизированный ответ PagedResponse<T>


## Структура проекта

Lab06-API/
├── Controllers/
│   └── ProductsController.cs
├── DTOs/
│   ├── ProductV1.cs
│   ├── ProductV2.cs
│   ├── PaginationRequest.cs
│   └── PagedResponse.cs
├── Validators/
│   └── ProductV2Validator.cs
├── Program.cs
└── Lab06-API.csproj

### Ключевые реализации
-Версионирование в Program.cs

builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
});

-Модель ProductV1 (базовая)

public class ProductV1
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    public string? Description { get; set; }
    [Range(0.01, 100000)] public decimal Price { get; set; }
    [Required] public string Category { get; set; }
}

-Модель ProductV2 (расширенная)

public class ProductV2 : ProductV1
{
    [Required] public string Brand { get; set; }
    [Range(0, 1000)] public int StockQuantity { get; set; }
    [Range(0, 5)] public double Rating { get; set; }
}

## скриншеты

-главная API 1
-главная API 2
-запрос 1 получить продукты V1 с пагинацией
-результат на запрос 1
-запрос 2 создать продукт V2
-результат  запрос 2
-запросна несуществующий id (ошибка 404)
-запрос фильтрация по категории
-результат фильтрации по категории
-невалидный код
-результат на невалидный код

## Инструкция по запуску

-Перейти в папку проекта

\лр_6\src\Lab06-API

- Восстановить зависимости:

dotnet restore

-Запустить приложение:

dotnet run

-Открыть в браузере:

http://localhost:5000/swagger
