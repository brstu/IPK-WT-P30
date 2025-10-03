Лабораторная работа 04 - ASP.NET Core MVC и сессии
Выполненные задания
✅ Создание проекта и настройка

    Тип проекта: ASP.NET Core MVC

    Endpoint healthz: /healthz возвращает { status: "ok" }

    Сессии: Настроены с timeout 30 минут

✅ Страницы приложения

    Главная (/) - информационная страница с навигацией

    Список товаров (/Items) - таблица с товарами

    Детали товара (/Items/Details/{id:int}) - страница с ограничением маршрута

    Поиск (/Items/Search) - демонстрация привязки модели из query

    Логин (/Home/Login) - форма авторизации

    Секретная страница (/Home/Secret) - защищённая страница

✅ Система авторизации

    Фиктивная аутентификация: Логин admin, пароль password

    Хранение в сессии:

        IsAuthenticated = "true"

        Username = "admin"

    Защита страниц: Проверка сессии перед доступом к /Home/Secret

✅ Привязка модели

    FromQuery: Параметры поиска name и minPrice

    FromRoute: ID товара в деталях {id:int}

    FromBody: Добавление нового товара (JSON)

Техническая реализация
Пайплайн middleware
text

Request → ExceptionHandler → HTTPS Redirection → StaticFiles → Routing → Session → Authorization → Endpoints

Ключевые компоненты

    Program.cs: Конфигурация сервисов и middleware

    Контроллеры: HomeController, ItemsController

    Модели: LoginModel, Item

    Представления: Razor pages с Bootstrap

Сессия
csharp

// Запись в сессию
HttpContext.Session.SetString("IsAuthenticated", "true");

// Чтение из сессии
var isAuth = HttpContext.Session.GetString("IsAuthenticated");

Проверка работы
Тестовые данные для входа

    Логин: admin

    Пароль: password

Маршруты для проверки

    GET /healthz - проверка работы API

    GET /Items/Details/1 - работа ограничения маршрута

    GET /Items/Search?name=Мышь&minPrice=10 - привязка из query

    GET /Home/Secret - проверка защиты (редирект на логин)

Запуск приложения
bash

dotnet run

Приложение доступно по адресу: https://localhost:7000
Особенности реализации

    Валидация маршрутов с {id:int}

    Обработка ошибок (404 для несуществующих товаров)

    JSON API для добавления товаров

    Поиск с фильтрацией по названию и цене

    Сессионные cookie с настройками безопасности

Скриншоты
(img/lp4_0.png)
(img/lp4_1.png)
(img/lp4_2.png)
(img/lp4_3.png)
        