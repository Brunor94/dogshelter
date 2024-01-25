namespace DogShelterService.Application.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using DogShelterService.Application.DTOs;
    using DogShelterService.Application.Queries;
    using DogShelterService.Application.Services;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Gateways.Interfaces;
    using DogShelterService.Domain.Queries.Interfaces;
    using DogShelterService.Domain.Repositories.Interfaces;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class DogServiceTests
    {
        private readonly Mock<ITheDogApiGateway> theDogApiGatewayMock;
        private readonly Mock<IDogRepository> dogRepositoryMock;
        private readonly Mock<IBreedRepository> breedRepositoryMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly DogService dogService;
        private readonly Fixture fixture;

        public DogServiceTests()
        {
            this.fixture = new Fixture();

            this.theDogApiGatewayMock = new Mock<ITheDogApiGateway>();
            this.dogRepositoryMock = new Mock<IDogRepository>();
            this.breedRepositoryMock = new Mock<IBreedRepository>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();

            this.dogService = new DogService(
                this.theDogApiGatewayMock.Object,
                this.dogRepositoryMock.Object,
                this.breedRepositoryMock.Object,
                this.unitOfWorkMock.Object);
        }

        [Fact]
        public async Task PostDogAsync_CreationRequestIsNull_ThrowArgumentNullException()
        {
            // Arrange
            DogCreationRequestDTO dogCreationRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => this.dogService.PostDogAsync(dogCreationRequest));

            this.theDogApiGatewayMock.Verify(x => x.GetBreedInfoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            this.breedRepositoryMock.Verify(x => x.GetBreedByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            this.dogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Dog>(), It.IsAny<CancellationToken>()), Times.Never);
            this.unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task PostDogAsync_BreedNameIsInvalid_ReturnNull()
        {
            // Arrange
            var dogCreationRequest = this.fixture.Create<DogCreationRequestDTO>();

            this.theDogApiGatewayMock.Setup(x => x.GetBreedInfoAsync(dogCreationRequest.Breed, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(null as Breed);

            // Act
            var result = await this.dogService.PostDogAsync(dogCreationRequest);

            // Assert
            result.Should().BeNull();

            this.theDogApiGatewayMock.Verify(x => x.GetBreedInfoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            this.breedRepositoryMock.Verify(x => x.GetBreedByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            this.dogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Dog>(), It.IsAny<CancellationToken>()), Times.Never);
            this.unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task PostDogAsync_CreationRequestIsValid_AddDogAndReturnNewDog()
        {
            // Arrange
            var dogCreationRequest = this.fixture.Create<DogCreationRequestDTO>();
            var dog = this.fixture.Create<Dog>();
            var breed = this.fixture.Create<Breed>();
            var affectedRows = 2;

            this.theDogApiGatewayMock.Setup(x => x.GetBreedInfoAsync(dogCreationRequest.Breed, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(breed);

            this.breedRepositoryMock.Setup(x => x.GetBreedByNameAsync(breed.Name, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(breed);

            this.dogRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Dog>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(dog);

            this.unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(affectedRows);

            // Act
            var result = await this.dogService.PostDogAsync(dogCreationRequest);

            // Assert
            result.Should().BeEquivalentTo(dog);

            this.theDogApiGatewayMock.Verify(x => x.GetBreedInfoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            this.breedRepositoryMock.Verify(x => x.GetBreedByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            this.dogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Dog>(), It.IsAny<CancellationToken>()), Times.Once);
            this.unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void GetDogsAsync_SearchRequestIsNull_ThrowArgumentNullException()
        {
            // Arrange
            ISearchDogsRequest searchDogsRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => this.dogService.GetDogsAsync(searchDogsRequest));
        }

        [Fact]
        public async void GetDogsAsync_SearchRequestIsValid_ReturnDogsList()
        {
            // Arrange
            var searchDogsRequest = fixture.Create<SearchDogsRequestDTO>();
            var dogsList = fixture.CreateMany<Dog>().ToList();

            dogRepositoryMock.Setup(x => x.GetAsync(searchDogsRequest, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(dogsList);

            // Act
            var result = await dogService.GetDogsAsync(searchDogsRequest);

            // Assert
            result.Should().BeEquivalentTo(dogsList);
            dogRepositoryMock.Verify(x => x.GetAsync(It.IsAny<ISearchDogsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}