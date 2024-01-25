namespace DogShelterService.Application.Queries
{
    using DogShelterService.Domain.Enums;
    using DogShelterService.Domain.Queries.Interfaces;

    public class SearchDogsRequestDTO : ISearchDogsRequest
    {
        public string DogName { get; set; }

        /// <summary>
        /// 0 - Small: Smaller than 35cm <br/>
        /// 1 - Medium: Between 35cm and 55cm <br/>
        /// 2 - Large: Bigger than 55cm <br/>
        /// </summary>
        public SizeCategory? SizeCategory { get; set; }

        public string BreedName { get; set; }
        public string Temperament { get; set; }

        public bool HasSearchFilters()
        {
            return !string.IsNullOrWhiteSpace(DogName)
                   || SizeCategory != null
                   || !string.IsNullOrWhiteSpace(BreedName)
                   || !string.IsNullOrWhiteSpace(Temperament);
        }
    }
}