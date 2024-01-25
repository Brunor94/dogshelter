namespace DogShelterService.Infrastructure.Tests.Gateways.Mappers
{
    using AutoFixture;
    using DogShelterService.Domain.Enums;
    using DogShelterService.Infrastructure.Gateways.Mappers;
    using DogShelterService.Infrastructure.Gateways.Models;
    using FluentAssertions;
    using Xunit;

    public class DogApiGatewayMapperTests
    {
        private readonly DogApiGatewayMapper mapper;
        private readonly Fixture fixture;

        public DogApiGatewayMapperTests()
        {
            this.mapper = new DogApiGatewayMapper();
            this.fixture = new Fixture();
        }

        [Fact]
        public void Map_InputIsNull_ReturnNull()
        {
            // Arrange
            DogApiGatewayDTO dogDto = null;

            // Act
            var result = this.mapper.Map(dogDto);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("0-34", SizeCategory.Small, 17)]
        [InlineData("35-55", SizeCategory.Medium, 45)]
        [InlineData("56-100", SizeCategory.Large, 78)]
        public void Map_GivenDogDto_ReturnMappedDog(string height, SizeCategory expectedCategory, double expectedAvgHeight)
        {
            // Arrange
            var dogDto =
                this.fixture.Build<DogApiGatewayDTO>()
                    .With(dogDto => dogDto.Height, new HeightApiGatewayDTO() { Metric = height })
                .Create();

            // Act
            var result = this.mapper.Map(dogDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dogDto.Name);
            string.Join(',', result.Temperaments).Should().BeEquivalentTo(dogDto.Temperament);
            result.SizeCategory.Should().Be(expectedCategory);
            result.AvgHeight.Should().Be(expectedAvgHeight);
        }
    }
}