namespace People.SelfHostedApi.Infrastructure.Formatters
{
    using System;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using Newtonsoft.Json;

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