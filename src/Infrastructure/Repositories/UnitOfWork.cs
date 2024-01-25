namespace DogShelterService.Infrastructure.Repositories
{
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Domain.Repositories.Interfaces;
    using DogShelterService.Infrastructure.Persistence;
    using EnsureThat;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DogShelterDbContext dogShelterDbContext;

        public UnitOfWork(DogShelterDbContext context)
        {
            EnsureArg.IsNotNull(context, nameof(context));

            this.dogShelterDbContext = context;
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return this.dogShelterDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}