Отчет по лабораторной работе №5
"ASPNET Core Identity + Слоистая архитектура. Сервис времени (DI)"
Овечкина Екатерина Васильевна
Группа: P30
Дата: 29.09.2025

🏗️ Архитектура проекта
Структура решения:
text

Task05/
├── Task05.Domain/           # Domain layer
│   ├── Entities/
│   └── Interfaces/
├── Task05.Infrastructure/   # Data layer  
│   ├── Data/
│   ├── Migrations/
│   └── Services/
├── Task05.Application/      # Business logic layer
└── Task05.Web/             # Presentation layer
    ├── Controllers/
    ├── Views/
    └── Program.cs

🔧 Реализованная функциональность
1. Слоистая архитектура ✅

    Domain layer: Содержит entities и interfaces

    Infrastructure layer: Реализация доступа к данным, миграции БД

    Web layer: Контроллеры, представления, конфигурация

2. Сервис времени (Dependency Injection) ✅

Интерфейс:
csharp

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}

Реализация:
csharp

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}

Регистрация в DI контейнере:
csharp

builder.Services.AddScoped<IDateTimeService, DateTimeService>();

3. ASP.NET Core Identity ✅

Модель пользователя:
csharp

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

Контекст базы данных:
csharp

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

4. Система аутентификации и авторизации ✅

Реализовано:

    Регистрация новых пользователей

    Вход в систему

    Выход из системы

    Ролевая модель (роль Admin)

    Защита контроллеров атрибутами [Authorize]

5. Защищенные разделы ✅

Контроллеры с авторизацией:
csharp

[Authorize(Roles = "Admin")]
public class FilesController : Controller

[Authorize(Roles = "Admin")]
public class HealthController : Controller

🗃️ База данных
Миграции:

    Создана база данных Task05

    Применены миграции для таблиц Identity

    Создана роль "Admin"

    Создан тестовый пользователь-администратор

Тестовый администратор:

    Email: admin@test.com

    Password: Admin123!

    Роль: Admin

🌐 Интерфейс приложения
Главная страница содержит:

    Отображение локального и UTC времени

    Статус аутентификации пользователя

    Навигацию по разделам

    Кнопки входа/регистрации/выхода

Защищенные разделы:

    Загрузка файлов - только для Admin

    Health Check - только для Admin

🔒 Безопасность
Реализованные меры:

    Хеширование паролей

    Защита от CSRF атак

    Валидация входных данных

    Ролевая авторизация

    Защита маршрутов

🚀 Запуск приложения
Команды для запуска:
bash

dotnet ef database update
dotnet run

Доступные URL:

    http://localhost:5159 - главная страница

    /Account/Login - страница входа

    /Account/Register - страница регистрации

    /Files/Index - загрузка файлов (только Admin)

    /Health/Check - проверка системы (только Admin)

📊 Результаты тестирования
Тест-кейсы:
Тест	Результат
Регистрация нового пользователя	✅ Успешно
Вход с правильными данными	✅ Успешно
Вход с неправильными данными	✅ Ошибка
Доступ к /Files без авторизации	✅ Запрещен
Доступ к /Files с ролью User	✅ Запрещен
Доступ к /Files с ролью Admin	✅ Разрешен
Отображение времени	✅ Корректно
🛠️ Технические детали
Используемые технологии:

    ASP.NET Core 6.0

    Entity Framework Core

    ASP.NET Core Identity

    SQL Server LocalDB

    Bootstrap 5

NuGet пакеты:

    Microsoft.AspNetCore.Identity.EntityFrameworkCore

    Microsoft.EntityFrameworkCore.SqlServer

    Microsoft.EntityFrameworkCore.Tools

    Microsoft.AspNetCore.Identity.UI

✅ Вывод

Лабораторная работа успешно завершена. Все поставленные задачи выполнены:

    ✅ Реализована слоистая архитектура

    ✅ Создан и зарегистрирован сервис времени через DI

    ✅ Интегрирована система ASP.NET Core Identity

    ✅ Настроена аутентификация и авторизация

    ✅ Реализована ролевая модель с защитой разделов

    ✅ Создана база данных с миграциями

    ✅ Разработан пользовательский интерфейс