namespace People.SelfHostedApi
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUserManager : UserManager<IdentityUser>
    {
        public ApplicationUserManager(IUserStore<IdentityUser> store) : base(store) { }
    }
}
