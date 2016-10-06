namespace People.SelfHostedApi.Controllers
{
    using System.Web.Http;
    using Microsoft.AspNet.Identity;

    [Authorize]
    public class UserController : ApiController
    {
        public ApplicationUserManager UserManager { get; set; }

        public IHttpActionResult Get()
        {
            var id = User.Identity.GetUserId();
            var user = UserManager.FindById(id);
            return Ok(user);
        }
    }
}