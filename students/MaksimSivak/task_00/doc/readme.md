# Лабораторная работа 00 — Методические материалы

**Выполнил:** Сивак Максим  
**Идентификатор:** MaksimSivak

## Установленное ПО и версии

- **Git:** $(git --version)
- **.NET SDK:** $(dotnet --version)
- **Node.js:** $(node --version) 
- **npm:** $(npm --version)
- **VS Code:** 1.88.1
- **SQL Server:** Microsoft SQL Server 2019 Express Edition

## Команды проверки
\`\`\`powershell
git --version
dotnet --version
node --version
npm --version
sqlcmd -S \`"(localdb)\MSSQLLocalDB\`" -E -Q \`"SELECT @@VERSION\`"