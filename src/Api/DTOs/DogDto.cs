namespace DogShelterService.Api.DTOs
{
    using System.Collections.Generic;
    using DogShelterService.Domain.Enums;

    public class DogDto
    {
        public string Name { get; set; }
        public string Breed { get; set; }
        public double AvgHeight { get; set; }
        public List<string> Temperaments { get; set; }
    }
}