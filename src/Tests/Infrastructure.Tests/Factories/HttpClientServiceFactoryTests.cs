namespace DogShelterService.Infrastructure.Tests.Factories
{
    using System;
    using System.Net.Http;
    using AutoFixture;
    using DogShelterService.Infrastructure.Factories;
    using DogShelterService.Infrastructure.Services;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class HttpClientServiceFactoryTests
    {
        private readonly Fixture fixture = new Fixture();
        private readonly Mock<IHttpClientFactory> ttpClientFactoryMock = new Mock<IHttpClientFactory>();

        private readonly HttpClientServiceFactory httpClientServiceFactory;

        public HttpClientServiceFactoryTests()
        {
            this.httpClientServiceFactory = new HttpClientServiceFactory(this.ttpClientFactoryMock.Object);
        }

        [Fact]
        public void Constructor_GivenNullHttpClientFactory_ThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new HttpClientServiceFactory(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateClient_GivenNullOrWhitespaceName_ThrowArgumentException(string name)
        {
            // Act
            Action act = () => this.httpClientServiceFactory.CreateClient(name);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateClient_GivenValidName_ReturnHttpClientService()
        {
            // Arrange
            var clientName = this.fixture.Create<string>();

            this.ttpClientFactoryMock.Setup(f => f.CreateClient(clientName))
                                      .Returns(new HttpClient());

            // Act
            var result = this.httpClientServiceFactory.CreateClient(clientName);

            // Assert
            result.Should().BeOfType<HttpClientService>();
        }

        [Fact]
        public void CreateClient_HttpClientFactoryReturnsNull_ThrowInvalidOperationException()
        {
            // Arrange
            var clientName = this.fixture.Create<string>();

            this.ttpClientFactoryMock.Setup(f => f.CreateClient(clientName))
                                      .Returns((HttpClient)null);

            // Act
            Action act = () => this.httpClientServiceFactory.CreateClient(clientName);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}