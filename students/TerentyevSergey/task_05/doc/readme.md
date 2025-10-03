Лабораторная работа 05 - DI/слои, Identity и файлы

Скриншоты
    (img/lp5_0.png)
    (img/lp5_1.png)
    (img/lp5_2.png)

Выполненные задания
✅ Слоистая архитектура

Реализована 4-слойная архитектура:

    Domain - интерфейсы предметной области (IClock)

    Application - бизнес-логика и интерфейсы (IDateTime)

    Infrastructure - реализации сервисов (DateTimeService)

    Web - контроллеры и представления

✅ Dependency Injection

Внедрён сервис времени через DI:

    IClock (Domain) → IDateTime (Application) → DateTimeService (Infrastructure)

    Зарегистрирован в Program.cs как Scoped сервис

    Внедряется в HomeController через конструктор

✅ Рабочее приложение

Главная страница отображает текущее время сервера, полученное через DI цепочку.
Техническая реализация
Схема слоёв
text

Task05.Domain/
└── Common/
    └── IClock.cs (interface)

    ↑
Task05.Application/  
└── Common/Interfaces/
    └── IDateTime.cs (interface)

    ↑  
Task05.Infrastructure/
└── Services/
    └── DateTimeService.cs (implementation)

    ↑
Task05.Web/
├── Controllers/
│   └── HomeController.cs (consumption)
├── Views/
│   └── Home/
│       └── Index.cshtml (display)
└── Program.cs (registration)

Ключевые компоненты

Domain Layer (IClock):
csharp

public interface IClock
{
    DateTime UtcNow { get; }
}

Application Layer (IDateTime):
csharp

public interface IDateTime
{
    DateTime Now { get; }
}

Infrastructure Layer (DateTimeService):
csharp

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}

DI Registration (Program.cs):
csharp

builder.Services.AddScoped<IDateTime, DateTimeService>();

Consumption (HomeController):
csharp

public HomeController(IDateTime dateTime)
{
    _dateTime = dateTime;
}

Проверка работы
Запуск приложения
bash

cd Task05.Web
dotnet run

Приложение доступно по адресу: http://localhost:5236
Тестирование DI

    Главная страница отображает текущее время сервера

    Время получается через полную цепочку DI

    Сообщение "DI is working!" подтверждает корректность внедрения

Особенности реализации

    Чёткое разделение ответственности между слоями

    Использование интерфейсов для слабой связанности

    Правильная регистрация зависимостей в контейнере DI

    Демонстрация работы паттерна Dependency Injection