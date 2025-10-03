using Microsoft.AspNetCore.Mvc;
using Moq;
using Task07.Api.Controllers;
using Task07.Api.Models;
using Task07.Api.Services;
using Microsoft.Extensions.Logging; // Добавляем эту директиву
using Xunit;

namespace Task07.Tests
{
    public class ScenarioControllerTests
    {
        private readonly ScenarioController _controller;
        private readonly Mock<IExternalApiService> _externalServiceMock;

        public ScenarioControllerTests()
        {
            _externalServiceMock = new Mock<IExternalApiService>();
            var loggerMock = new Mock<ILogger<ScenarioController>>();
            
            _controller = new ScenarioController(_externalServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task CompleteScenario_ReturnsSuccessResult()
        {
            // Arrange
            var mockPosts = new List<Post>
            {
                new Post { Id = 1, UserId = 1, Title = "Test Post", Body = "Test Body" }
            };
            
            _externalServiceMock.Setup(x => x.GetPostsAsync())
                .ReturnsAsync(mockPosts);
            _externalServiceMock.Setup(x => x.GetPostAsync(1))
                .ReturnsAsync(mockPosts[0]);

            // Act
            var result = await _controller.CompleteScenario();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void GetCachedProducts_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetCachedProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}