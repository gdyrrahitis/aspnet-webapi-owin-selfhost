namespace People.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Context;
    using Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var password = "pass123";
                var userStore = new UserStore<IdentityUser>(context);
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = new IdentityUser
                {
                    Email = "CustomUser1@email.com",
                    UserName = "CustomUser1",
                };
                userManager.Create(user, password);
            }


            context.People.AddOrUpdate(p => p.Name, 
                new Person { Name = "Andrew Peters", Age = 35 },
                new Person { Name = "Brice Lambson", Age = 40 },
                new Person { Name = "Rowan Miller", Age = 41 });

            base.Seed(context);
        }
    }
}
