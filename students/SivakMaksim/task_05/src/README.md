- Лабораторная работа по ASP.NET Core
- Тема: Разработка веб-приложения на ASP.NET Core MVC
- Описание проекта
Веб-приложение разработано на ASP.NET Core с использованием архитектуры MVC. Проект включает базовую структуру с контроллерами, представлениями и поддержкой Identity.

🛠 Технологии
ASP.NET Core 8.0

Entity Framework Core

MVC Architecture

Identity для аутентификации

SQL Server

- Структура проекта
text
Task05.Web/
├── Controllers/          # Контроллеры MVC
├── Views/               # Представления Razor
├── Models/              # Модели данных
├── Data/                # Контекст базы данных
├── Properties/          # Настройки запуска
├── wwwroot/             # Статические файлы
├── Program.cs           # Точка входа
└── Task05.Web.csproj    # Файл проекта
- Запуск приложения
Предварительные требования
.NET 8.0 SDK

SQL Server (LocalDB или Express)

Git (опционально)

1. Клонирование и настройка
bash
# Перейдите в папку проекта
cd D:\Task05\Task05.Web

# Восстановите зависимости
dotnet restore
2. Настройка базы данных
bash
# Создайте миграции (если есть)
dotnet ef migrations add InitialCreate
dotnet ef database update
3. Запуск приложения
bash
# Запуск на стандартном порту
dotnet run

# Запуск на конкретном порту
dotnet run --urls="http://localhost:5594"

# Запуск без профиля launchSettings
dotnet run --no-launch-profile --urls="http://localhost:5594"
4. Открытие в браузере
После успешного запуска откройте:

Основное приложение: http://localhost:5594

Страница входа: http://localhost:5594/Identity/Account/Login

- Конфигурация
Настройки в appsettings.json
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Task05DB;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
Изменение порта
Измените в Properties/launchSettings.json:

json
"applicationUrl": "http://localhost:5594"
- Тестирование
Проверка работы сервера
bash
# Проверка доступности порта
netstat -ano | findstr :5594

# Проверка запущенных процессов
tasklist | findstr dotnet
В случае ошибок
bash
# Остановка всех процессов dotnet
taskkill /f /im dotnet.exe

# Очистка и пересборка
dotnet clean
dotnet build

- (Частые проблемы)
1. Порт занят
bash
# Используйте другой порт
dotnet run --urls="http://localhost:5000"
2. Ошибка базы данных
bash
# Удалите и пересоздайте базу
dotnet ef database drop
dotnet ef database update
3. Отсутствуют зависимости
bash
# Восстановите пакеты
dotnet restore