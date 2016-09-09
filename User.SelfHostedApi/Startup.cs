namespace User.SelfHostedApi
{
    using System;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Controllers;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    using Newtonsoft.Json;
    using Owin;

    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();

            configuration.MapHttpAttributeRoutes();
            configuration.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { controller = nameof(PeopleController), id = RouteParameter.Optional });

            configuration.Formatters.Add(new BrowserFormatter());

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token")
            });
            app.UseWebApi(configuration);
        }

        public class BrowserFormatter : JsonMediaTypeFormatter
        {
            public BrowserFormatter()
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
                SerializerSettings.Formatting = Formatting.Indented;
            }

            public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
        }
    }
}