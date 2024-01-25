namespace DogShelterService.Infrastructure.Configurations.Interfaces
{
    public interface IRetryPolicyConfiguration
    {
        int MaxRetry { get; }
        int PauseBetweenFailuresInSeconds { get; }
    }
}