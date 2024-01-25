namespace DogShelterService.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Common.Services.Interfaces;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Queries.Interfaces;
    using DogShelterService.Domain.Repositories.Interfaces;
    using DogShelterService.Infrastructure.Persistence;
    using DogShelterService.Infrastructure.Repositories.Entities;
    using DogShelterService.Infrastructure.Repositories.Mappers.Interfaces;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore;

    public sealed class DogRepository : IDogRepository
    {
        private readonly DogShelterDbContext context;
        private readonly IDogEntityMapper dogEntityMapper;
        private readonly IRetryService retryService;

        public DogRepository(DogShelterDbContext context, IDogEntityMapper dogEntityMapper, IRetryService retryService)
        {
            EnsureArg.IsNotNull(context, nameof(context));
            EnsureArg.IsNotNull(dogEntityMapper, nameof(dogEntityMapper));
            EnsureArg.IsNotNull(retryService, nameof(retryService));

            this.context = context;
            this.dogEntityMapper = dogEntityMapper;
            this.retryService = retryService;
        }

        public async Task<Dog> AddAsync(Dog dog, CancellationToken cancellationToken = default)
        {
            var dogEntity = this.dogEntityMapper.Map(dog);

            var result =
                await retryService.RetryAsync(async token => await this.context.Dogs.AddAsync(dogEntity, token),
                                              cancellationToken)
                                  ;

            return this.dogEntityMapper.Map(result.Entity);
        }

        public async Task<List<Dog>> GetAsync(ISearchDogsRequest searchDogsRequest, CancellationToken cancellationToken = default)
        {
            var query = this.BuildDogsSearchQuery(searchDogsRequest);
            query = query.Include(d => d.Breed);

            var dogEntities =
                await retryService.RetryAsync(async token => await query.ToListAsync(token),
                                              cancellationToken)
                                  ;

            return dogEntityMapper.Map(dogEntities);
        }

        private IQueryable<DogEntity> BuildDogsSearchQuery(ISearchDogsRequest searchDogsRequest)
        {
            var query = context.Dogs.AsQueryable();

            if (searchDogsRequest.HasSearchFilters())
            {
                if (searchDogsRequest.SizeCategory != null)
                {
                    query = query.Where(dog => dog.Breed.SizeCategory == (int)searchDogsRequest.SizeCategory);
                }

                if (!string.IsNullOrEmpty(searchDogsRequest.DogName))
                {
                    query = query.Where(dog => dog.Name.ToLower() == searchDogsRequest.DogName.ToLower());
                }

                if (!string.IsNullOrWhiteSpace(searchDogsRequest.BreedName))
                {
                    query = query.Where(dog => dog.Breed.Name.ToLower().Contains(searchDogsRequest.BreedName.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(searchDogsRequest.Temperament))
                {
                    query = query.Where(dog => dog.Breed.Temperaments.ToLower().Contains(searchDogsRequest.Temperament.ToLower()));
                }
            }

            return query;
        }
    }
}