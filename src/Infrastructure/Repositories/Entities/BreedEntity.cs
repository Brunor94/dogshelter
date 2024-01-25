namespace DogShelterService.Infrastructure.Repositories.Entities
{
    using System.Collections.Generic;

    public class BreedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Temperaments { get; set; }
        public int SizeCategory { get; set; }
        public double AvgHeight { get; set; }

        public ICollection<DogEntity> Dogs { get; set; }
    }
}