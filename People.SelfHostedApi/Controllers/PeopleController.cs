namespace People.SelfHostedApi.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Domain.Entities;
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

        public IHttpActionResult Put(int id, Person person)
        {
            if (person == null)
                return BadRequest();

            var persistedEntity = _service.GetPerson(id);
            if (persistedEntity == null)
                return NotFound();

            persistedEntity.Name = person.Name;
            persistedEntity.Age = person.Age;

            _service.Update(persistedEntity);
            return Ok(persistedEntity);
        }

        public IHttpActionResult Post(Person person)
        {
            if (person == null)
                return BadRequest();

            _service.Create(person);
            return Created($"{Request.RequestUri.AbsoluteUri}/{person.Id}", person);
        }

        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest($"{nameof(id)} is not valid.");

            var person = _service.GetPerson(id);
            if (person == null)
                return NotFound();

            _service.Delete(person);
            return Ok();
        }
    }
}