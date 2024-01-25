namespace DogShelterService.Domain.Queries.Interfaces
{
    using DogShelterService.Domain.Enums;

    public interface ISearchDogsRequest
    {
        string DogName { get; set; }

        SizeCategory? SizeCategory { get; set; }

        string BreedName { get; set; }

        string Temperament { get; set; }

        bool HasSearchFilters();
    }
}