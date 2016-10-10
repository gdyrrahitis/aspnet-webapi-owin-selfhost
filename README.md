# aspnet-webapi-owin-selfhost
An ASP.NET Web API 2 OWIN self-hosted application, with standard and secure `ApiController`'s.

# Code
Download or clone the repository in your local machine.
Solution has been developed in Visual Studio 2015.

## Api
The ASP.NET Web API application. It consumes the underlying services, People.Services, to communicate with the underlying data store.

## Core
Contains all core libraries.
- *People.Domain*. Contains Entity Framework Code First entities, as well as repositories which provide some data access layer to the underlying store.
- *People.Services*. Contains services which consume the data access layer, People.Domain, and expose business logic.

## Test.Suites
Contains all unit tests for the application. Tests are grouped per project, so there are tests for:
- *People.Domain*
- *People.Services*
- *People.SelfHosted.Api*

Used NUnit 3 framework for unit tests.

# License
Apache License Version 2.0
