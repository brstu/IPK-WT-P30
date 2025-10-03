using Microsoft.AspNetCore.Mvc;
using Task07.Api.Controllers;
using Task07.Api.Models;
using Xunit;

namespace Task07.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _controller = new ProductsController();
        }

        [Fact]
        public void GetProducts_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void GetProduct_WithValidId_ReturnsProduct()
        {
            // Arrange
            var testId = 1;

            // Act
            var result = _controller.GetProduct(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(testId, product.Id);
        }

        [Fact]
        public void GetProduct_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var result = _controller.GetProduct(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateProduct_ReturnsCreatedResult()
        {
            // Arrange
            var newProduct = new Product 
            { 
                Name = "Test Product", 
                Description = "Test Description", 
                Price = 19.99m,
                Stock = 10
            };

            // Act
            var result = _controller.CreateProduct(newProduct);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var product = Assert.IsType<Product>(createdResult.Value);
            Assert.Equal(newProduct.Name, product.Name);
        }

        [Fact]
        public void GetProducts_ReturnsListOfProducts()
        {
            // Act
            var result = _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.True(products.Count >= 3);
        }
    }
}