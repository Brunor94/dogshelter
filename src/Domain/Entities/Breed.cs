namespace DogShelterService.Domain.Entities
{
    using System.Collections.Generic;
    using DogShelterService.Domain.Enums;

    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Temperaments { get; set; }
        public SizeCategory SizeCategory { get; set; }
        public double AvgHeight { get; set; }
    }
}