namespace DogShelterService.Infrastructure.Persistence
{
    using DogShelterService.Infrastructure.Repositories.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DogShelterDbContext : DbContext
    {
        public DbSet<DogEntity> Dogs { get; set; }
        public DbSet<BreedEntity> Breeds { get; set; }

        public DogShelterDbContext(DbContextOptions<DogShelterDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("DogShelter");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DogEntity>(entity =>
            {
                entity.Property(dog => dog.Name).IsRequired();
                entity.HasOne(dog => dog.Breed)
                      .WithMany(breed => breed.Dogs)
                      .HasForeignKey(dog => dog.BreedId);
            });

            modelBuilder.Entity<BreedEntity>(entity =>
            {
                entity.Property(breed => breed.Name).IsRequired();

                entity.HasIndex(breed => breed.Name).IsUnique();
            });
        }
    }
}