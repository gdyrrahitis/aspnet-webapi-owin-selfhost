# aspnet-webapi-owin-selfhost
An ASP.NET Web API 2 OWIN self-hosted application, with standard and secure `ApiController`'s.

# Code
Much of this code is explained on following blog posts, [tour in different abstraction levels](http://codereform.com/blog/post/unit-testing-and-code-coverage-for-asp-net-web-api-12/) as well as [tour on unit tests and code coverage](http://codereform.com/blog/post/unit-testing-and-code-coverage-for-asp-net-web-api-22/).

API is selfhosted with OWIN, running in `Startup.cs`
```
public class Startup
{
    public static void Configuration(IAppBuilder app)
    {
        IContainer container;
        var configuration = new HttpConfiguration();
        AutofacConfig.Register(configuration, out container);
        WebApiRouteConfig.Register(configuration);

        app.UseAutofacMiddleware(container);
        app.UseAutofacWebApi(configuration);
        app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
        {
            AllowInsecureHttp = true,
            TokenEndpointPath = new PathString("/token"),
            AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            Provider = configuration.DependencyResolver.GetService(typeof(CustomAuthorizationServerProvider)) as CustomAuthorizationServerProvider
        });
        app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        app.UseWebApi(configuration);
    }
}
```

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

## Code coverage
For code coverage OpenCover is used.
To run the code coverage reports, navigate to `/bin/Debug` folder of each test project and open the `_RunCodeCoverageInOutput.bat`. This will kick off the OpenCover tool to start making coverage reports in XML format.

Download or clone the repository in your local machine.

*Solution has been developed in Visual Studio 2015.*

# License
Apache License Version 2.0
