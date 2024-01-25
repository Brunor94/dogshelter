namespace DogShelterService.Api.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using DogShelterService.Api.Controllers;
    using DogShelterService.Api.DTOs;
    using DogShelterService.Api.Mappers.Interfaces;
    using DogShelterService.Application.DTOs;
    using DogShelterService.Application.Queries;
    using DogShelterService.Application.Services.Interfaces;
    using DogShelterService.Domain.Entities;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class DogsControllerTests
    {
        private readonly Mock<IDogService> dogServiceMock;
        private readonly Mock<IDogDtoMapper> dogDtoMapperMock;
        private readonly DogsController dogsController;
        private readonly Fixture fixture;

        public DogsControllerTests()
        {
            this.fixture = new Fixture();
            this.dogServiceMock = new Mock<IDogService>();
            this.dogDtoMapperMock = new Mock<IDogDtoMapper>();
            this.dogsController = new DogsController(
                this.dogServiceMock.Object,
                this.dogDtoMapperMock.Object);
        }

        [Fact]
        public async void GetDogsAsync_NoDogs_ReturnNotFound()
        {
            // Arrange
            var searchDogsRequestDTO = this.fixture.Create<SearchDogsRequestDTO>();
            var dogs = this.fixture.CreateMany<Dog>(1).ToList();
            var dogsDto = this.fixture.CreateMany<DogDto>(1).ToList();

            this.dogServiceMock.Setup(service => service.GetDogsAsync(searchDogsRequestDTO, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new List<Dog>());

            // Act
            var result = await this.dogsController.GetDogsAsync(searchDogsRequestDTO);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>()
                         .Which.Value.Should().BeNull();

            this.dogServiceMock.Verify(service => service.GetDogsAsync(It.IsAny<SearchDogsRequestDTO>(), It.IsAny<CancellationToken>()), Times.Once);
            this.dogDtoMapperMock.Verify(x => x.Map(It.IsAny<List<Dog>>()), Times.Never);
        }

        [Fact]
        public async void GetDogsAsync_DogsExist_ReturnOkWithDogs()
        {
            // Arrange
            var searchDogsRequestDTO = this.fixture.Create<SearchDogsRequestDTO>();
            var dogs = this.fixture.CreateMany<Dog>(1).ToList();
            var dogsDto = this.fixture.CreateMany<DogDto>(1).ToList();

            this.dogServiceMock.Setup(service => service.GetDogsAsync(searchDogsRequestDTO, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(dogs);

            this.dogDtoMapperMock.Setup(x => x.Map(dogs))
                                 .Returns(dogsDto);

            // Act
            var result = await this.dogsController.GetDogsAsync(searchDogsRequestDTO);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                         .Which.Value.Should().BeEquivalentTo(dogsDto);

            this.dogServiceMock.Verify(service => service.GetDogsAsync(It.IsAny<SearchDogsRequestDTO>(), It.IsAny<CancellationToken>()), Times.Once);
            this.dogDtoMapperMock.Verify(x => x.Map(It.IsAny<List<Dog>>()), Times.Once);
        }

        [Fact]
        public async Task SaveDogsAsync_SaveSuccessful_ReturnCreated()
        {
            // Arrange
            var dogRequest = this.fixture.Create<DogCreationRequestDTO>();
            var dog = this.fixture.Create<Dog>();
            var dogDto = this.fixture.Create<DogDto>();

            this.dogServiceMock.Setup(service => service.PostDogAsync(dogRequest, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(dog);

            this.dogDtoMapperMock.Setup(x => x.Map(dog))
                                 .Returns(dogDto);

            // Act
            var result = await dogsController.SaveDogsAsync(dogRequest, new CancellationToken());

            // Assert
            result.Result.Should().BeOfType<ObjectResult>()
                        .Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
            ((ObjectResult)result.Result).Value.Should().BeEquivalentTo(dogDto);

            this.dogServiceMock.Verify(service => service.PostDogAsync(It.IsAny<DogCreationRequestDTO>(), It.IsAny<CancellationToken>()), Times.Once);
            this.dogDtoMapperMock.Verify(x => x.Map(It.IsAny<Dog>()), Times.Once);
        }

        [Fact]
        public async Task SaveDogsAsync_SaveFails_ReturnBadRequest()
        {
            // Arrange
            var dogRequest = this.fixture.Create<DogCreationRequestDTO>();
            var dog = null as Dog;

            dogServiceMock.Setup(service => service.PostDogAsync(dogRequest, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(dog);

            // Act
            var result = await dogsController.SaveDogsAsync(dogRequest, new CancellationToken());

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();

            this.dogServiceMock.Verify(service => service.PostDogAsync(It.IsAny<DogCreationRequestDTO>(), It.IsAny<CancellationToken>()), Times.Once);
            this.dogDtoMapperMock.Verify(x => x.Map(It.IsAny<Dog>()), Times.Never);
        }
    }
}