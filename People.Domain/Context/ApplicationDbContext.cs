namespace People.Domain.Context
{
    using System.Data.Entity;
    using Entities;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(): base("DefaultConnection", throwIfV1Schema: false) { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Person> People { get; set; }
    }
}