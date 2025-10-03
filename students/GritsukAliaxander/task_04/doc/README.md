№ Лабораторная работа 04 - Интернет-магазин электроники
Описание проекта
ASP.NET Core MVC приложение для интернет-магазина электроники с аутентификацией через сессии.

Функциональность
Главная страница с популярными товарами

Страница со списком всех товаров

Детальная страница товара с ограничением маршрута {id:int}

Система аутентификации (фиктивная)

Защищенная страница заказов

Привязка моделей из формы

Запуск проекта

dotnet run --urls="http://localhost:5000"
Приложение будет доступно по адресу: http://localhost:5000

Тестовые данные для входа:
Логин: admin

Пароль: password

Проверка health endpoint:

http://localhost:5000/healthz
Скриншоты
1. Главная страница


URL: http://localhost:5000

Показывает популярные товары

Навигационное меню

Статус аутентификации

2. Страница входа


URL: http://localhost:5000/Home/Login

Форма аутентификации

Тестовые данные: admin/password

3. Успешная аутентификация


Сообщение "Вы вошли как администратор"

Появление кнопки "Мои заказы"

4. Защищенная страница заказов


URL: http://localhost:5000/Home/Orders

Доступна только после входа

Форма создания заказа

5. Health Check Endpoint


URL: http://localhost:5000/healthz

Возвращает "Healthy"

6. Список всех товаров


URL: http://localhost:5000/Home/List

Полный каталог товаров

7. Детали товара с ограничением {id:int}


URL: http://localhost:5000/Home/Details/1

Ограничение маршрута только для integer ID

8. Создание заказа (привязка модели)


Демонстрация привязки модели из формы

Отображение данных после отправки

9. Консоль запуска приложения


Команда dotnet run

Порт прослушивания: http://localhost:5000

Схема пайплайна запроса
text
Запрос браузера
        ↓
Middleware Pipeline
    ├── Static Files
    ├── Routing
    ├── Session ← (проверка IsAuthenticated)
    └── Authorization
        ↓
Controller Processing
    ├── HomeController.Index()
    ├── HomeController.Login() [HttpPost]
    ├── HomeController.Orders() [CustomAuth]
    └── Model Binding (Form → OrderModel)
        ↓
View Rendering
    ├── Razor Pages
    ├── Layout (_Layout.cshtml)
    └── Session Data в View
        ↓
HTML Response
Проверка сессии
Как работает аутентификация:
Хранение в сессии: HttpContext.Session.SetString("IsAuthenticated", "true")

Проверка доступа: Атрибут [CustomAuth] проверяет сессию

Время жизни: 30 минут (настроено в Program.cs)

Данные сессии: Username, IsAuthenticated

Тестовый сценарий:
Перейти на /Home/Login

Ввести: admin / password

Проверить появление сообщения об успешном входе

Убедиться что доступна страница /Home/Orders

Проверить что без входа страница заказов недоступна

Код проверки сессии:
csharp
// Установка сессии при входе
HttpContext.Session.SetString("IsAuthenticated", "true");

// Проверка в атрибуте
var isAuthenticated = context.HttpContext.Session.GetString("IsAuthenticated");
if (isAuthenticated != "true") {
    context.Result = new RedirectToActionResult("Login", "Home", null);
}
Структура проекта
Controllers/ - HomeController с действиями и атрибутом авторизации

Models/ - Product, LoginModel, OrderModel (привязка данных)

Views/Home/ - Razor страницы с использованием сессии

Program.cs - конфигурация пайплайна и сервисов (сессии)

Middleware - кастомная аутентификация через сессии

Технические особенности
Сессии: DistributedMemoryCache для хранения состояния

Маршрутизация: Ограничение {id:int} для Details страницы

Привязка моделей: Form → OrderModel в CreateOrder

Авторизация: Кастомный атрибут [CustomAuth]

Health Check: Endpoint /healthz для мониторинга