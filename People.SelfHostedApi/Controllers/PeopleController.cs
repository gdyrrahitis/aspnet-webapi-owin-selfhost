namespace People.SelfHostedApi.Controllers
{
    using System.Web.Http;

    [Authorize]
    public class PeopleController: ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok("people");
        }
    }
}