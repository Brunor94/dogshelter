namespace DogShelterService.Infrastructure.Gateways.Models
{
    public class DogApiGatewayDTO
    {
        public WeightApiGatewayDTO Weight { get; set; }
        public HeightApiGatewayDTO Height { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string BredFor { get; set; }
        public string BreedGroup { get; set; }
        public string LifeSpan { get; set; }
        public string Temperament { get; set; }
        public string ReferenceImageId { get; set; }
    }
}