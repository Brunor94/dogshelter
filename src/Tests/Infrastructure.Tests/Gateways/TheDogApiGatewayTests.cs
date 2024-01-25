namespace DogShelterService.Infrastructure.Tests.Gateways
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using DogShelterService.Common.Services.Interfaces;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Infrastructure.Factories.Interfaces;
    using DogShelterService.Infrastructure.Gateways;
    using DogShelterService.Infrastructure.Gateways.Mappers.Interfaces;
    using DogShelterService.Infrastructure.Gateways.Models;
    using DogShelterService.Infrastructure.Services.Interfaces;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class TheDogApiGatewayTests
    {
        private readonly Mock<IHttpClientServiceFactory> httpClientServiceFactoryMock;
        private readonly Mock<IDogApiGatewayMapper> dogApiGatewayMapperMock;
        private readonly Mock<IRetryService> retryServiceMock;
        private readonly Mock<IHttpClientService> httpClientServiceMock;
        private readonly Fixture fixture;

        private readonly TheDogApiGateway theDogApiGateway;

        public TheDogApiGatewayTests()
        {
            this.httpClientServiceFactoryMock = new Mock<IHttpClientServiceFactory>();
            this.dogApiGatewayMapperMock = new Mock<IDogApiGatewayMapper>();
            this.retryServiceMock = new Mock<IRetryService>() { CallBase = true };
            this.httpClientServiceMock = new Mock<IHttpClientService>();
            this.fixture = new Fixture();

            this.httpClientServiceFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(this.httpClientServiceMock.Object);

            this.theDogApiGateway = new TheDogApiGateway(
                this.httpClientServiceFactoryMock.Object,
                this.dogApiGatewayMapperMock.Object,
                this.retryServiceMock.Object);
        }

        [Fact]
        public void Constructor_CalledWithNullDependencies_ThrowArgumentNullException()
        {
            // Arrange
            Action act1 = () => _ = new TheDogApiGateway(null, this.dogApiGatewayMapperMock.Object, this.retryServiceMock.Object);
            Action act2 = () => _ = new TheDogApiGateway(this.httpClientServiceFactoryMock.Object, null, this.retryServiceMock.Object);
            Action act3 = () => _ = new TheDogApiGateway(this.httpClientServiceFactoryMock.Object, this.dogApiGatewayMapperMock.Object, null);

            // Act & Assert
            act1.Should().Throw<ArgumentNullException>();
            act2.Should().Throw<ArgumentNullException>();
            act3.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetBreedInfoAsync_GivenNullOrWhitespaceInput_ThrowArgumentException()
        {
            // Act
            Func<Task> act = async () => await this.theDogApiGateway.GetBreedInfoAsync(null);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetBreedInfoAsync_GivenValidInput_ReturnDog()
        {
            // Arrange
            var dogBreed = this.fixture.Create<string>();
            var dogsApiGatewayDTO = this.fixture.CreateMany<DogApiGatewayDTO>(1);
            var breed = this.fixture.Create<Breed>();

            this.retryServiceMock
                .Setup(r => r.RetryAsync(It.IsAny<Func<CancellationToken, Task<IEnumerable<DogApiGatewayDTO>>>>(), It.IsAny<CancellationToken>()))
                .Returns((Func<CancellationToken, Task<IEnumerable<DogApiGatewayDTO>>> func, CancellationToken token) => func(token));

            this.httpClientServiceMock.Setup(s => s.GetAsync<IEnumerable<DogApiGatewayDTO>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(dogsApiGatewayDTO);

            this.dogApiGatewayMapperMock.Setup(m => m.Map(dogsApiGatewayDTO.First()))
                                        .Returns(breed);

            // Act
            var result = await this.theDogApiGateway.GetBreedInfoAsync(dogBreed);

            // Assert
            result.Should().BeEquivalentTo(breed);

            this.httpClientServiceMock.Verify(
                s => s.GetAsync<IEnumerable<DogApiGatewayDTO>>(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Once);

            this.retryServiceMock.Verify(m => m.RetryAsync(It.IsAny<Func<CancellationToken, Task<IEnumerable<DogApiGatewayDTO>>>>(), It.IsAny<CancellationToken>()), Times.Once);

            this.dogApiGatewayMapperMock.Verify(m => m.Map(It.IsAny<DogApiGatewayDTO>()), Times.Once);

        }
    }
}