namespace People.Domain.Context
{
    using System.Data.Entity;
    using Entities;

    public class ApplicationDbContext: BaseDbContext
    {
        public ApplicationDbContext(): base("DefaultConnection", throwIfV1Schema: false) { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Person> People { get; set; }
    }
}