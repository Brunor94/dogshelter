namespace DogShelterService.Infrastructure.Repositories.Entities
{
    public class DogEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int BreedId { get; set; }

        public BreedEntity Breed { get; set; }
    }
}