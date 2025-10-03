using Polly;
using Polly.Timeout;

namespace Task07.Application.Services;

public class ExternalService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalService> _logger;

    public ExternalService(HttpClient httpClient, ILogger<ExternalService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetExternalDataAsync(string url)
    {
        var policy = Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning("Retry {RetryCount} after {TimeSpan} seconds for {Url}", 
                        retryCount, timespan.TotalSeconds, url);
                });

        return await policy.ExecuteAsync(async () =>
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        });
    }
}