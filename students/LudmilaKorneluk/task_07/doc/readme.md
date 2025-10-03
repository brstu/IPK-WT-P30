Отчет по лабораторной работе №7
"Разработка Web API на ASP.NET Core"
Овечкина Екатерина Васильевна
Группа: P30
Дата: 29.09.2025
🔧 Выполненные задачи
1. Создание структуры проекта

Разработана многослойная архитектура по принципу Clean Architecture:
text

Task07.API/          # Web API с контроллерами
Task07.Application/  # Бизнес-логика и сервисы  
Task07.Core/         # Абстракции и доменные модели
Task07.Infrastructure/ # Реализации репозиториев

2. Реализация моделей и интерфейсов

Core/Entities/Item.cs
csharp

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; }
}

Core/Interfaces/IItemService.cs
csharp

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllItemsAsync();
    Task<Item?> GetItemByIdAsync(int id);
    Task<Item> CreateItemAsync(Item item);
    Task<Item?> UpdateItemAsync(int id, Item item);
    Task<bool> DeleteItemAsync(int id);
}

3. Реализация сервисов

Application/Services/ItemService.cs - бизнес-логика приложения

Infrastructure/Data/MockItemRepository.cs - временное хранилище данных
4. Создание контроллера

Controllers/ItemsController.cs - REST API endpoints:

    GET /api/items - получить все задачи

    GET /api/items/{id} - получить задачу по ID

    POST /api/items - создать новую задачу

    PUT /api/items/{id} - обновить задачу

    DELETE /api/items/{id} - удалить задачу

5. Настройка приложения

Program.cs - конфигурация DI контейнера и middleware:
csharp

// Регистрация сервисов
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemRepository, MockItemRepository>();

// Swagger документация
builder.Services.AddSwaggerGen();

// Настройка pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

🧪 Тестирование API
Проверка всех endpoints:

1. GET все задачи:
powershell

GET http://localhost:5283/api/items

Результат:
json

[
  {
    "id": 1,
    "name": "Первая задача",
    "description": "Описание первой задачи",
    "createdAt": "2025-09-29T22:13:19.2641451Z",
    "isCompleted": false
  }
]

2. GET задача по ID:
powershell

GET http://localhost:5283/api/items/1

3. POST создать задачу:
powershell

POST http://localhost:5283/api/items
Body: {
  "name": "Новая задача",
  "description": "Описание новой задачи", 
  "isCompleted": false
}

4. PUT обновить задачу:
powershell

PUT http://localhost:5283/api/items/1
Body: {
  "name": "Обновленная задача",
  "description": "Новое описание",
  "isCompleted": true
}

5. DELETE удалить задачу:
powershell

DELETE http://localhost:5283/api/items/2

Swagger документация:

Доступна по адресу: http://localhost:5283/swagger
🏗️ Архитектурные решения
1. Clean Architecture

    Разделение ответственности между слоями

    Core не зависит от внешних слоев

    Легкость тестирования и поддержки

2. Dependency Injection

    Автоматическое разрешение зависимостей

    Легкая замена реализаций (Mock → Database)

    Упрощение unit-тестирования

3. Repository Pattern

    Абстракция доступа к данным

    Возможность легкой замены источника данных

    Изоляция бизнес-логики от инфраструктуры

📊 Результаты
✅ Достигнутые цели:

    Создана многослойная архитектура Web API

    Реализованы все CRUD операции

    Настроен Dependency Injection

    Добавлена Swagger документация

    Протестированы все endpoints

    Решены проблемы циклических зависимостей

🔄 Коды ответов HTTP:

    200 OK - успешные GET, PUT запросы

    201 Created - успешное создание через POST

    404 Not Found - ресурс не найден

    204 No Content - успешное удаление

🚀 Возможности расширения

    База данных - замена MockRepository на Entity Framework

    Валидация - добавление Data Annotations к моделям

    Пагинация - для больших списков задач

    Аутентификация - JWT tokens для защиты API

    Кэширование - повышение производительности

    Логирование - structured logging

    Docker - контейнеризация приложения

📝 Выводы

В ходе лабораторной работы были успешно изучены и применены на практике:

    Принципы построения многослойной архитектуры в ASP.NET Core

    Реализация REST API с полным набором CRUD операций

    Настройка Dependency Injection для управления зависимостями

    Создание автоматической документации API с помощью Swagger

    Решение проблем циклических зависимостей в сложных проектах

Разработанное Web API соответствует современным стандартам и может быть легко расширено дополнительным функционалом. Архитектура проекта обеспечивает легкую поддержку, тестирование и масштабирование приложения.
🖼️ Демонстрация работы

https://swagger-screenshot.png
Интерфейс Swagger с документацией API

https://api-testing.png
Тестирование API endpoints через PowerShell

Работа завершена успешно! ✅
