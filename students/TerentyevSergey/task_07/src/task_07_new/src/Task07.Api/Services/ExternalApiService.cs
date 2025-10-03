using System.Text.Json;

namespace Task07.Api.Services
{
    public interface IExternalApiService
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post?> GetPostAsync(int id);
    }

    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalApiService> _logger;

        public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching posts from external API");
                
                // Используем абсолютный URL или убеждаемся, что BaseAddress установлен
                var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<Post>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                _logger.LogInformation("Successfully fetched {Count} posts", posts?.Count ?? 0);
                return posts ?? new List<Post>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching posts from external API");
                throw;
            }
        }

        public async Task<Post?> GetPostAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching post {PostId} from external API", id);
                
                // Используем абсолютный URL
                var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/posts/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var post = JsonSerializer.Deserialize<Post>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return post;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching post {PostId} from external API", id);
                throw;
            }
        }
    }

    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}