namespace People.SelfHostedApi.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using Domain.Entities;

    public class PeopleController : ApiController
    {
        private readonly DbContext _context;

        public PeopleController(DbContext context)
        {
            _context = context;
        }

        public IHttpActionResult Get()
        {
            var people = _context.Set<Person>().ToList();
            return Ok(people);
        }

        public IHttpActionResult Get(int id)
        {
            var person = _context.Set<Person>().Find(id);
            return Ok(person);
        }
    }
}