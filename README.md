# <p align="middle"> Hairdresser Booking Api
## About
 Hairdresser Booking Api is a REST API application created to improve work efficiency of hairdresser salon. Application provides a salon manager a possibility to manage workers, their work time for specific days and services they are performing. Api assures clients required informations about workers and possible services. Application not only enables clients to make reservation, but also suggest best time for a visit. 
  
  Project was written in ASP.NET Core using Visual Studio 2022.
 
  
## Technologies
  #### Api
  * .NET 6
  * ASP.NET Core
  * Entity Framework Core 6
  * FluentValidation 10.3.6
  * JWT Authentication 6.0.2
  * AutoMapper 11.0.0
  * NLog 4.14.0
  * Swagger 6.2.3
  * Depedency Injection
  * MSSQL
  * CORS
  
  #### Tests
  * xunit
  * Moq
  * FluentAssertions
  
  
## Features
  * Authentication by JWT token
  * Authorizations by Claims
  * Entites managment by CRUD methods
  * Errors and critical moments logging
  * Middleware to handle exceptions
  * Different strategies to suggest best time for a reservation (usage of Strategy Design Pattern)
  * Mapping beetween database entities and Dto objects
  * Requests data validation by fluentValidation and data annotations
  * Seeding database with basic data
  * Many to many and one to many relations in database managed by ORM (wrote in code-first approach)
  * Unit tests for business logic
  * Integration tests for all controllers methods and validators
  
 ## Instalation
  To run this project it is necessary to change connecting string value in `appsettings.json` file which is in api project. After that use command `update-database` in Package Manager Console. After that application should run properly. Default `CORS` policy will allow only `http://localhost8080` origin. In order to change it, open appsettings.json file and replace value for `AllowedOrigins`.By default swagger documentation should be opened after running application.
