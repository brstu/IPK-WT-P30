using Microsoft.AspNetCore.Mvc;
using Moq;
using Task07.Application.Interfaces;
using Task07.Domain.Entities;
using Task07.Web.Controllers;
using Xunit;

namespace Task07.Tests.Controllers;

public class ItemsControllerTests : ControllerTestBase
{
    private readonly Mock<IItemService> _mockItemService;
    private readonly ItemsController _controller;

    public ItemsControllerTests()
    {
        _mockItemService = new Mock<IItemService>();
        _controller = new ItemsController(_mockItemService.Object);
    }

    [Fact]
    public async void GetItems_ReturnsOkResult_WithListOfItems()
    {
        // Arrange
        var testItems = GetTestItems();
        _mockItemService.Setup(service => service.GetAllItemsAsync())
            .ReturnsAsync(testItems);

        // Act
        var result = await _controller.GetItems();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Item>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
        
        // Проверка сериализации
        var firstItem = returnValue.First();
        Assert.Equal("Test Item 1", firstItem.Name);
        Assert.Equal(1, firstItem.Id);
    }

    [Fact]
    public async void GetItem_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var testItem = GetTestItems().First();
        _mockItemService.Setup(service => service.GetItemByIdAsync(1))
            .ReturnsAsync(testItem);

        // Act
        var result = await _controller.GetItem(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Item>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal("Test Item 1", returnValue.Name);
    }

    [Fact]
    public async void GetItem_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockItemService.Setup(service => service.GetItemByIdAsync(999))
            .ReturnsAsync((Item)null);

        // Act
        var result = await _controller.GetItem(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async void CreateItem_ValidItem_ReturnsCreatedResult()
    {
        // Arrange
        var newItem = new Item { Name = "New Item", Description = "New Description" };
        var createdItem = new Item { Id = 3, Name = "New Item", Description = "New Description", CreatedAt = DateTime.UtcNow };
        
        _mockItemService.Setup(service => service.CreateItemAsync(It.IsAny<Item>()))
            .ReturnsAsync(createdItem);

        // Act
        var result = await _controller.CreateItem(newItem);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ItemsController.GetItem), createdResult.ActionName);
        Assert.Equal(3, ((Item)createdResult.Value).Id);
    }

    [Fact]
    public async void CreateItem_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var invalidItem = new Item { Name = "", Description = "Description" }; // Невалидное имя
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.CreateItem(invalidItem);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}