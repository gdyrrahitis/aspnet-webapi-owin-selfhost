namespace People.SelfHostedApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public class People
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}