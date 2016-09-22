namespace People.SelfHostedApi.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    [Authorize]
    public class UserController : ApiController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager => 
            _userManager ?? (_userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>());

        public IHttpActionResult Get()
        {
            var id = User.Identity.GetUserId();
            var user = UserManager.FindById(id);
            return Ok(user);
        }
    }
}