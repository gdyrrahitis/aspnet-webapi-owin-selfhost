namespace People.SelfHostedApi
{
    using System;
    using System.Web.Http;
    using Autofac;
    using Infrastructure.Security;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    using Owin;

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
}