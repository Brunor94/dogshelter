namespace DogShelterService.Domain.Entities
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Breed Breed { get; set; }
    }
}