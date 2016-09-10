namespace People.SelfHostedApi.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Database;

    public class PeopleController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public PeopleController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult Get()
        {
            var people = _context.People.ToList();
            return Ok(people);
        }

        public IHttpActionResult Get(int id)
        {
            var person = _context.People.Find(id);
            return Ok(person);
        }
    }
}