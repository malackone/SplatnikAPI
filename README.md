Splatnik is an original web/mobile application dedicated to manage your home finances. Main business process is based on creating your monthly home budget, add expenses/incomes, manage all credits, installments, debts and savings in one place. Basing on user's data Splatnik will help you to see the "big picture" of your finances, plan future expenses like vacations, home renovations etc. User will have access panel for BI and analitics charts and summaries to see all nessessery informations.


All project is divided into three parts:



Part I - Web API (Status: In progress)

REST API, "core" of the project. All business logic, user management, data access layer etc. will be put here. API is developed in .NET Core 3.1 framework with additional libraries like EF Core, AutoMapper, Fluent Validation. Authentication and authorization is based on ASP.NET Core Identity, but in further development progress 3rd party authentication providers like Facebook or Google will be added. Splatnik's database technology is SQL Server.


Part II - Web Application (Status: Design)

Single page application (SPA) will be main application part, where users will have access to all planned functionalities in Splatnik. Current work progress is in design phase - I am working on the UI/UX/application layout. Propably part of the styles will be combination of Bootstrap and Material Design frameworks.


Part III - Mobile App - (Status: To Be Done In The Future/Design)

If web application will pass the "users' satisfaction test" next part of Splatnik will be mobile application. Technology has not been chosen yet, but propably Flutter will be the one.
