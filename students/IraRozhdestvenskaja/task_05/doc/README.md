# Лабораторная работа 05

## Слои/DI, ASP.NET Core Identity, загрузка/скачивание файлов

**Выполнил:** [Бортюк И.А.]  
**Группа:** [П-30] 

## Цель работы

Освоить принципы слоистой архитектуры, Dependency Injection, работу с ASP.NET Core Identity и реализацию загрузки/скачивания файлов в веб-приложениях.

## Архитектура решения

### Схема слоёв
Task05.sln
├── Task05.Web (ASP.NET Core MVC)
│ ├── Controllers
│ ├── Views
│ ├── Data (DbContext)
│ └── Services
├── Task05.Application (Class Library)
│ ├── Interfaces
│ └── Dtos
├── Task05.Infrastructure (Class Library)
│ └── Services
└── Task05.Domain (Class Library)
├── Models
└── Interfaces

### Зависимости:
- Web → Application, Infrastructure
- Application → Domain  
- Infrastructure → Application, Domain

### Назначение слоёв:
-Domain - бизнес-модели и интерфейсы
-Application - логика приложения, DTO, интерфейсы сервисов
-Infrastructure - реализация сервисов, работа с файловой системой
-Web - представление, контроллеры, настройка приложения

## Реализованная функциональность

### Слоистая архитектура и DI.
-Интерфейсы в Domain:

csharp
public interface IClock
{
    DateTime UtcNow { get; }
}

-Реализации в Infrastructure:
csharp
public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}

-Регистрация сервисов:

csharp
builder.Services.AddScoped<IClock, SystemClock>();
builder.Services.AddScoped<IFileService, FileService>();

### ASP.NET Core Identity

-DbContext:

csharp
public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<FileUpload> FileUploads { get; set; }
}

-Инициализация ролей:

csharp
public class RoleInitializerService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Создание ролей Admin и User
        // Создание пользователя admin@example.com
    }
}

### Работа с файлами

-Сервис для работы с файлами:

csharp
public class FileService : IFileService
{
    public async Task<FileUpload?> UploadFileAsync(Stream fileStream, 
        string fileName, string contentType, string userId)
    {
        // Генерация безопасного имени
        var storedFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        // Сохранение файла
        // Возврат метаданных
    }
}

-Контроллер:

csharp
[Authorize]
public class FileController : Controller
{
    [HttpPost]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB
    public async Task<IActionResult> Upload(IFormFile file)
    {
        // Валидация и загрузка файла
    }
}

## Система ролей и авторизации

### Роли пользователей

Роль	Права доступа
User	Регистрация, вход, загрузка файлов
Admin	Все права User + доступ к админ-панели

### Авторизация в контроллерах

csharp
[Authorize] // Только для авторизованных
public class FileController : Controller

[Authorize(Roles = "Admin")] // Только для администраторов
public IActionResult Admin()

### Тестовые пользователи

-Администратор:

Email: admin@example.com

Пароль: Admin123!

-Обычный пользователь:

Регистрация через форму /Identity/Account/Register


## Работа с файлами

### Валидация загружаемых файлов

sharp
// Проверка расширений
var allowedExtensions = new[] { ".txt", ".pdf", ".doc", ".docx", ".jpg", ".png" };

// Ограничение размера
[RequestSizeLimit(10 * 1024 * 1024)]

### Безопасное хранение
Безопасные имена: Guid + расширение

Изоляция: файлы в wwwroot/uploads

Валидация: проверка типа и размера перед сохранением

## База данных и миграции

### Миграции Entity Framework

// Создание миграции
dotnet ef migrations add InitIdentity

// Применение миграции
dotnet ef database update

### Созданные таблицы
AspNetUsers - пользователи

AspNetRoles - роли

AspNetUserRoles - связь пользователей и ролей

FileUploads - метаданные файлов




## Запуск программы

### Предварительные требования

-.NET 9.0 SDK
-Entity Framework Core Tools

### Команды запуска

// Переходим в папку Web проекта
cd C:\Users\Irinka\Desktop\ЛР_5\Task05\Task05.Web

// Восстановление зависимостей
dotnet restore

// Сборка решения
dotnet build

// Создание и применение миграций
cd Task05.Web
dotnet ef migrations add InitIdentity
dotnet ef database update

// Запуск приложения
dotnet run --urls="http://localhost:5000"

**Видим
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.

**Открываем в браузере

-https://localhost:7000 (рекомендуется)

-http://localhost:5000

если порт занят, используем другой порт
dotnet run --urls="http://localhost:5000"
