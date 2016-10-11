namespace People.SelfHostedApi.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Services.Person;

    public class PeopleController : ApiController
    {
        private readonly IPersonService _service;

        public PeopleController(IPersonService service)
        {
            _service = service;
        }

        public IHttpActionResult Get()
        {
            var people = _service.GetPeople().ToList();
            if (people.Any())
                return Ok(people);

            return NotFound();
        }

        public IHttpActionResult Get(int id)
        {
            if (id < 0) return BadRequest($"{nameof(id)} is not valid.");

            var person = _service.GetPerson(id);
            if (person == null)
                return NotFound();

            return Ok(person);
        }
    }
}