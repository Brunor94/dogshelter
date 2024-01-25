namespace DogShelterService.Api.Tests.Controllers
{
    using DogShelterService.Api.Controllers;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class HealthControllerTests
    {
        [Fact]
        public void GetHealth_CheckingWealth_AvailableStatus()
        {
            // Arrange
            var controller = new HealthController();

            // Act
            var result = controller.GetHealth();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<dynamic>();
        }
    }
}