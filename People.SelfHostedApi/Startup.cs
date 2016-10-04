namespace People.SelfHostedApi
{
    using System;
    using System.Web.Http;
    using Domain.Context;
    using Infrastructure.Security;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    using Owin;

    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationDbContext>(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            var configuration = new HttpConfiguration();
            WebApiRouteConfig.Register(configuration);
            AutofacConfig.Register(configuration);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CustomAuthorizationServerProvider()
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            app.UseWebApi(configuration);
        }
    }
}