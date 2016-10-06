namespace People.SelfHostedApi.Infrastructure.Security
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.OAuth;

    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public ApplicationUserManager UserManager { get; set; }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult(0);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = UserManager.Find(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "Username and password do not match.");
                return Task.FromResult(0);
            }

            var identity = GenerateClaimsIdentity(context, user);
            context.Validated(identity);
            return Task.FromResult(0);
        }

        private static ClaimsIdentity GenerateClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context,
            IdentityUser user)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            return identity;
        }
    }
}