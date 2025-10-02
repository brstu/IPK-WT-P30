Отчет по лабораторной работе №6
"REST API с версионированием, Swagger и валидацией"
Овечкина Екатерина Васильевна
Группа: P30
Дата: 29.09.2025
📋 Цель работы

Разработать RESTful Web API с поддержкой версионирования, документацией через Swagger и корректной валидацией входных данных.
🏗️ Архитектура проекта
Структура решения:
text

Task06/
├── Task06.API/                 # Presentation layer
│   ├── Controllers/
│   │   ├── V1/ItemsController.cs
│   │   └── V2/ItemsController.cs
│   ├── Models/
│   │   ├── V1/ItemDto.cs
│   │   └── V2/ItemDto.cs
│   ├── Services/
│   └── Program.cs
├── Task06.Core/                # Domain layer
│   ├── Entities/
│   ├── Interfaces/
│   ├── DTOs/
│   └── Common/
├── Task06.Application/         # Business logic layer
└── Task06.Infrastructure/      # Data access layer

🔧 Реализованная функциональность
1. Версионирование API ✅

Настройка в Program.cs:
csharp

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

Контроллеры с версиями:

    [ApiVersion("1.0")] + [Route("api/v{version:apiVersion}/[controller]")]

    [ApiVersion("2.0")] + расширенный функционал

2. Модели данных по версиям ✅

Версия 1 (Task06.API/Models/V1/ItemDto.cs):
csharp

public class ItemDto
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Name { get; set; }
    [Required, StringLength(500)] public string Description { get; set; }
    [Range(0.01, 10000)] public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}

Версия 2 (Task06.API/Models/V2/ItemDto.cs):
csharp

public class ItemDto
{
    // Все поля из v1 +
    [StringLength(50)] public string? Category { get; set; }
    [Range(0, 5)] public double? Rating { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime? UpdatedAt { get; set; }
}

3. Пагинация, фильтрация, сортировка ✅

Параметры запроса:

    page - номер страницы (по умолчанию: 1)

    pageSize - размер страницы (по умолчанию: 10)

    sort - поле сортировки (name, price, date, rating)

    filter - фильтр по названию/описанию

    category - фильтр по категории (только v2)

Реализация в контроллерах:
csharp

public ActionResult<IEnumerable<ItemDto>> GetItems(
    [FromQuery] string? filter,
    [FromQuery] string? sort = "name",
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)

4. Swagger документация ✅

Настройка в Program.cs:
csharp

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Task06 API", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Task06 API", Version = "v2" });
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task06 API v1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Task06 API v2");
});

5. Валидация и обработка ошибок ✅

DataAnnotations валидация:
csharp

[Required]
[StringLength(100)]
[Range(0.01, 10000)]

Обработка ошибок через ProblemDetails:
csharp

options.InvalidModelStateResponseFactory = context =>
{
    var problemDetails = new ValidationProblemDetails(context.ModelState)
    {
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        Title = "Validation error",
        Status = StatusCodes.Status400BadRequest,
        Detail = "Please refer to the errors property",
        Instance = context.HttpContext.Request.Path
    };
    return new BadRequestObjectResult(problemDetails);
};

🌐 API Endpoints
Версия 1.0:
Method	URL	Description
GET	/api/v1/items	Получить список с пагинацией
GET	/api/v1/items/{id}	Получить по ID
POST	/api/v1/items	Создать новый item
PUT	/api/v1/items/{id}	Обновить item
DELETE	/api/v1/items/{id}	Удалить item
Версия 2.0:
Method	URL	Description
GET	/api/v2/items	Расширенный список с фильтрацией по категории
GET	/api/v2/items/search/tags	Поиск по тегам
+	Все методы из v1 с дополнительными полями	
🛠️ Технические детали
Используемые технологии:

    ASP.NET Core 9.0

    Microsoft.AspNetCore.Mvc.Versioning

    Swashbuckle.AspNetCore (Swagger)

    FluentValidation.AspNetCore

    DataAnnotations

NuGet пакеты:
bash

Microsoft.AspNetCore.Mvc.Versioning
Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
Swashbuckle.AspNetCore
FluentValidation.AspNetCore

📊 Тестирование API
Тест-кейсы:
Тест	Версия	Результат
GET /api/v1/items	v1.0	✅ Успешно
POST /api/v1/items (валидные данные)	v1.0	✅ Успешно
POST /api/v1/items (невалидные данные)	v1.0	✅ 400 Error
GET /api/v2/items?category=tech	v2.0	✅ Успешно
GET /api/v2/items/search/tags?tags=tech	v2.0	✅ Успешно
Swagger UI	обе	✅ Доступен
Пример запроса v1:
http

POST /api/v1/items
Content-Type: application/json

{
  "name": "Test Item",
  "description": "Test Description",
  "price": 99.99
}

Пример запроса v2:
http

POST /api/v2/items
Content-Type: application/json

{
  "name": "Test Item v2",
  "description": "Test Description v2",
  "price": 149.99,
  "category": "electronics",
  "rating": 4.5,
  "tags": ["tech", "gadget"]
}

✅ Выводы

Лабораторная работа успешно завершена. Все поставленные задачи выполнены:

    ✅ Созданы REST API контроллеры с версионированием

    ✅ Реализована пагинация, фильтрация и сортировка

    ✅ Подключена Swagger документация для обеих версий

    ✅ Настроена валидация через DataAnnotations

    ✅ Реализована обработка ошибок через ProblemDetails

    ✅ Создана слоистая архитектура проекта

API полностью функционально и соответствует требованиям лабораторной работы. Документация доступна через Swagger UI, поддерживаются стандарты REST и HTTP.
