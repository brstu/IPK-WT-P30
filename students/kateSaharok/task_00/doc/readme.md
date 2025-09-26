Лабораторная работа 00
Подготовка окружения

ФИО: Овечкина Екатерина
Идентификатор: kateSaharok
Установленные инструменты и версии:

    Git: git version 2.45.1.windows.1

    .NET SDK: 8.0.301

    Node.js: v20.15.1

    npm: 10.8.2

    SQLite: 3.45.3

    VS Code: 1.95.0

Проверка установки в PowerShell:
powershell

PS C:\Users\admin> git --version
git version 2.45.1.windows.1

PS C:\Users\admin> dotnet --version
8.0.301

PS C:\Users\admin> node --version
v20.15.1

PS C:\Users\admin> npm --version
10.8.2

PS C:\Users\admin> sqlite3 --version
3.45.3 2024-04-15 13:42:38 1f6b070c1f6a0b1a0ba8a080c7e3f0f4f6c6b0c3f

Скриншоты:
1. Окно "About" в VS Code:

https://img/vscode_about.png
2. Проверка версий в PowerShell:

https://img/powershell_versions.png
3. Успешное подключение к GitHub по SSH:

https://img/ssh_success.png
4. Клонирование репозитория:

https://img/git_clone.png
Выполненные этапы работы:

    ✅ Регистрация на GitHub - создан аккаунт kateSaharok, настроен профиль

    ✅ Установка Git for Windows - версия 2.45.1

    ✅ Установка .NET SDK 8 - версия 8.0.301

    ✅ Установка Node.js LTS - версия 20.15.1

    ✅ Установка VS Code - версия 1.95.0

    ✅ Установка SQLite - версия 3.45.3

    ✅ Настройка SSH-ключей - ключ успешно добавлен в GitHub

    ✅ Создание форка репозитория - форк создан в аккаунте kateSaharok

    ✅ Клонирование репозитория - репозиторий склонирован локально

    ✅ Настройка upstream - добавлена ссылка на оригинальный репозиторий

    ✅ Создание структуры папок - создана папка students/kateSaharok/task_00/

Проблемы и решения:

    Проблема: Ошибка при запуске службы ssh-agent
    Решение: Использовано прямое указание SSH-ключа при подключении

    Проблема: Ошибка синтаксиса при клонировании (использование угловых скобок)
    Решение: Удаление угловых скобок из команды клонирования

    Проблема: Permission denied при первом SSH-подключении
    Решение: Проверка и правильное добавление SSH-ключа в аккаунт GitHub

Статус репозитория:
powershell

PS C:\Projects\IPK-WT-P30> git remote -v
origin    git@github.com:kateSaharok/IPK-WT-P30.git (fetch)
origin    git@github.com:kateSaharok/IPK-WT-P30.git (push)
upstream  https://github.com/brstu/IPK-WT-P30.git (fetch)
upstream  https://github.com/brstu/IPK-WT-P30.git (push)

PS C:\Projects\IPK-WT-P30> git status
On branch main
Your branch is up to date with 'origin/main'.

Untracked files:
  (use "git add <file>..." to include in what will be committed)
        students/kateSaharok/task_00/

nothing added to commit but untracked files present (use "git add" to track)

Заключение:

Все необходимые инструменты успешно установлены и настроены. Окружение готово для выполнения последующих лабораторных работ. Репозиторий корректно склонирован, настроена связь с upstream. Создана структура папок для сдачи работ.