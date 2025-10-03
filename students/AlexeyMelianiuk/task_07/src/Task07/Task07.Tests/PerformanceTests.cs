using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Task07.Tests;

public class PerformanceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public PerformanceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetItems_PerformanceTest()
    {
        var responseTimes = new List<long>();
        var requestCount = 100;

        for (int i = 0; i < requestCount; i++)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            var response = await _client.GetAsync("/api/items");
            response.EnsureSuccessStatusCode();
            
            stopwatch.Stop();
            responseTimes.Add(stopwatch.ElapsedMilliseconds);
        }

        responseTimes.Sort();
        var p95 = responseTimes[(int)(requestCount * 0.95)];

        // До оптимизации: обычно > 50ms, после: < 20ms
        Assert.True(p95 < 50, $"P95 response time {p95}ms exceeds threshold");
        
        Console.WriteLine($"P95 Response Time: {p95}ms");
        Console.WriteLine($"Min: {responseTimes.First()}ms, Max: {responseTimes.Last()}ms");
        Console.WriteLine($"Average: {responseTimes.Average():F2}ms");
    }
}