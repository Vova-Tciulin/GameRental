# GameRental 

## Описание

Данный проект представляет собой веб-платформу, которая позволяет арендовать игровые аккаунты для игровых приставок. С помощью этой платформы пользователи имеют возможность легко находить доступные аккаунты, выбирать игры и приставки, а также арендовать их на определенный период времени.

**Данный проект реализованн с использованием .NET 7 и базы данных MS SQL.**

## Технологии (Backend)

- .NET7
- ASP.Net Core MVC
- Entity Framework Core
- MSSQL
- ASP.Net Identity
- NUnit
- Moq
- AutoMapper
- FluentValidation

## Технологии (Frontend)

- Html/Css
- JS
- Bootstrap
- Summernote

## Архитектура

Данный проект использует луковую архитектуру и состоит из слоев:

 -  Domain. Данный слой содержит все сущности бд, а так же интерфейсы репозиториев.
 -  Application. Данный слой содержит логику приложения 
 -  Infrastructure. Слой, который содержит реализации репозиториев, сам контекст бд, а также почтовый сервис.
 -  Web. Слой, который использует логику из слоя application и отправляет данные веб-клиенту. 

## Запуск проекта

- Клонировать проект: https://github.com/Vova-Tciulin/GameRental.git
- создать базу данных, использую комадку в консоли: dotnet ef database update


## UI 
**Часть изображений работы веб-сайта**
<div style="display: flex; justify-content: center; align-items: center; ">
 <p>Основная страница:</p>
 <img src="images/first.png" width="600"   > 
 <p>Страница корзины:</p>
 <img src="images/second.png" width="600" >
 <p>Информация о заказах:</p>
 <img src="images/thrid.png" width="600" >
 <p>Информациях об играх:</p>
 <img src="images/four.png" width="600">
 <p>Информация о доступных акканутах:</p>
 <img src="images/five.png" width="600" >
</div>

