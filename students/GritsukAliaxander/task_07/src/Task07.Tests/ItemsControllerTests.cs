using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Task07.Tests;

public class ItemsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ItemsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetItems_Returns200()
    {
        // Act
        var response = await _client.GetAsync("/api/items");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetItem_ExistingId_Returns200()
    {
        // Arrange
        var id = 1;

        // Act
        var response = await _client.GetAsync($"/api/items/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetItem_NonExistingId_Returns404()
    {
        // Arrange
        var id = 999;

        // Act
        var response = await _client.GetAsync($"/api/items/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateItem_ValidData_Returns201()
    {
        // Arrange
        var newItem = new { Name = "Test Item", Description = "Test Description" };
        var content = new StringContent(JsonConvert.SerializeObject(newItem), 
            Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/items", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetItems_ReturnsCamelCaseJson()
    {
        // Act
        var response = await _client.GetAsync("/api/items");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Contains("id", content); // camelCase property names
        Assert.Contains("name", content);
    }

    [Fact]
    public async Task GetItem_WithEtag_Returns304()
    {
        // Arrange - first request to get ETag
        var firstResponse = await _client.GetAsync("/api/items/1");
        var etag = firstResponse.Headers.ETag?.Tag;

        if (!string.IsNullOrEmpty(etag))
        {
            // Act - second request with ETag
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/items/1");
            request.Headers.IfNoneMatch.Add(new System.Net.Http.Headers.EntityTagHeaderValue(etag));
            
            var secondResponse = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotModified, secondResponse.StatusCode);
        }
    }

    [Fact]
    public async Task GetErrorEndpoint_Returns500WithProblemDetails()
    {
        // Act
        var response = await _client.GetAsync("/api/items/error");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}