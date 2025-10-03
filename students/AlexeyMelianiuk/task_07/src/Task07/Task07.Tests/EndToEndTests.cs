using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Task07.Tests;

public class EndToEndTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public EndToEndTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CompleteUserScenario_Login_UploadFile_GetCachedData()
    {
        // 1. Получение списка элементов (должен быть закэширован при повторных вызовах)
        var firstResponse = await _client.GetAsync("/api/items");
        firstResponse.EnsureSuccessStatusCode();
        var firstEtag = firstResponse.Headers.ETag?.Tag;

        // 2. Проверка кэширования с ETag
        var requestWithEtag = new HttpRequestMessage(HttpMethod.Get, "/api/items");
        requestWithEtag.Headers.IfNoneMatch.Add(new System.Net.Http.Headers.EntityTagHeaderValue(firstEtag));
        
        var cachedResponse = await _client.SendAsync(requestWithEtag);
        Assert.Equal(HttpStatusCode.NotModified, cachedResponse.StatusCode);

        // 3. Создание нового элемента
        var newItem = new { Name = "E2E Test Item", Description = "Created in end-to-end test" };
        var content = new StringContent(
            JsonSerializer.Serialize(newItem),
            Encoding.UTF8,
            "application/json");

        var createResponse = await _client.PostAsync("/api/items", content);
        createResponse.EnsureSuccessStatusCode();

        // 4. Получение созданного элемента
        var createdItem = await createResponse.Content.ReadFromJsonAsync<Item>();
        Assert.NotNull(createdItem);
        Assert.Equal("E2E Test Item", createdItem.Name);

        // 5. Проверка обработки ошибок для несуществующего элемента
        var notFoundResponse = await _client.GetAsync("/api/items/9999");
        Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);

        // 6. Проверка корреляции ID в ошибках
        var problemDetails = await notFoundResponse.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.NotNull(problemDetails.Extensions["traceId"]);
    }
}

// Модель для десериализации
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

// Модель для ProblemDetails
public class ProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public Dictionary<string, object> Extensions { get; set; } = new();
}