using Microsoft.AspNetCore.Mvc;
using Moq;
using Task07.API.Controllers;
using Task07.Core.Interfaces;
using Xunit;

namespace Task07.API.Tests
{
    public class ItemsControllerTests
    {
        [Fact]
        public void GetItems_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IItemService>();
            var controller = new ItemsController(mockService.Object);
            
            // Act
            var result = controller.Get();
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}