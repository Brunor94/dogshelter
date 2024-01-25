namespace DogShelterService.Infrastructure.Configurations
{
    using DogShelterService.Infrastructure.Configurations.Interfaces;

    public class GatewaysSettings : IGatewaysSettings
    {
        public GatewayConfig TheDogApiGateway { get; set; }
    }
}