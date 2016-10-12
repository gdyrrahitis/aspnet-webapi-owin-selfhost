namespace People.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
    }
}