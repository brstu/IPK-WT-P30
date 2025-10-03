Лабораторная работа 06 - REST API с версионированием и валидацией

Скриншоты:

(lp6_0.png) Swagger V1
(lp6_1.png) Swagger V2
(lp6_2.png) 
(lp6_3.png)
(lp6_4.png)

Выполненные задания
✅ REST API с атрибутом [ApiController]

    Контроллеры возвращают стандартные HTTP коды

    Используется ProblemDetails для ошибок (400, 404, 500)

    Автоматическая валидация моделей

✅ Версионирование API v1 и v2

    Маршруты: api/v1/items и api/v2/items

    В v2 добавлены новые поля: Category, CreatedAt

    Поддержка пагинации, сортировки и фильтрации

✅ Пагинация, фильтрация, сортировка

    Параметры: page, pageSize, sort, filter

    Сортировка: по имени, цене, категории (v2)

    Фильтрация: по названию и категории (v2)

✅ Swagger документация

    Подключен Swashbuckle.AspNetCore

    Отдельная документация для v1 и v2

    Описаны схемы DTO и примеры запросов

✅ Валидация данных

    DataAnnotations для базовой валидации

    FluentValidation для сложных правил

    Автоматическое возвращение ValidationProblem

Техническая реализация
Архитектура API
text

Controllers/
├── v1/ItemsController.cs    (базовая версия)
└── v2/ItemsController.cs    (расширенная версия)

Models/
├── ItemDto.cs              (v1 DTO)
├── ItemDtoV2.cs            (v2 DTO)  
└── CreateItemRequest.cs    (модель для создания)

Validators/
└── CreateItemRequestValidator.cs (правила валидации)

Ключевые endpoints

v1:

    GET /api/v1/items - список с пагинацией

    GET /api/v1/items/{id} - детали элемента

    POST /api/v1/items - создание элемента

v2:

    GET /api/v2/items - список с фильтрацией

    GET /api/v2/items/{id} - детали с доп. полями

Валидация
csharp

// DataAnnotations
[Required]
[StringLength(100)]
[Range(0.01, 10000)]

// FluentValidation
RuleFor(x => x.Name).NotEmpty().Length(3, 100)
RuleFor(x => x.Price).GreaterThan(0)

Примеры использования
Успешные запросы
http

GET /api/v1/items?page=1&pageSize=10&sort=price
GET /api/v2/items?filter=electronics&sort=category
POST /api/v1/items
{
  "name": "New Item",
  "description": "Description",
  "price": 25.99
}

Ошибки валидации
json

{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "errors": {
    "Name": ["Name is required"],
    "Price": ["Price must be greater than 0"]
  }
}

Ошибки 404
json

{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Item with id 999 was not found"
}

Запуск и тестирование
bash

dotnet run

Swagger UI: http://localhost:5007/swagger

Тестовые endpoints:

    http://localhost:5007/api/v1/items

    http://localhost:5007/api/v2/items

Особенности реализации

    Чистая архитектура с разделением версий

    Стандартизированные ответы и ошибки

    Гибкая система запросов с пагинацией и фильтрацией

    Полная документация через Swagger

    Комплексная валидация входных данных
        