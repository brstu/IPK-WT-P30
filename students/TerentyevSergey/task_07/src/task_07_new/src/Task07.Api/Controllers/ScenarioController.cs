using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Task07.Api.Models;
using Task07.Api.Services;

namespace Task07.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScenarioController : ControllerBase
    {
        private readonly IExternalApiService _externalService;
        private readonly ILogger<ScenarioController> _logger;
        private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Description = "Gaming laptop", Price = 999.99m, Stock = 10 },
            new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, Stock = 50 }
        };

        public ScenarioController(IExternalApiService externalService, ILogger<ScenarioController> logger)
        {
            _externalService = externalService;
            _logger = logger;
        }

        [HttpPost("complete-scenario")]
        public async Task<IActionResult> CompleteScenario()
        {
            var scenarioResults = new List<object>();

            try
            {
                _logger.LogInformation("Starting end-to-end scenario");

                // 1. Получаем локальные продукты (кэшируются)
                var products = _products;
                scenarioResults.Add(new { Step = 1, Action = "Get local products", Data = products });

                // 2. Создаем новый продукт
                var newProduct = new Product
                {
                    Name = "Scenario Product",
                    Description = "Created in E2E scenario",
                    Price = 49.99m,
                    Stock = 25
                };
                _products.Add(newProduct);
                scenarioResults.Add(new { Step = 2, Action = "Create new product", Data = newProduct });

                // 3. Получаем посты из внешнего API (с Polly retry)
                var posts = await _externalService.GetPostsAsync();
                scenarioResults.Add(new
                {
                    Step = 3,
                    Action = "Fetch external posts with Polly",
                    Count = posts.Count,
                    SamplePost = posts.FirstOrDefault()
                });

                // 4. Обновляем продукт
                var productToUpdate = _products.First();
                productToUpdate.Price = 899.99m;
                scenarioResults.Add(new
                {
                    Step = 4,
                    Action = "Update product price",
                    ProductId = productToUpdate.Id,
                    NewPrice = productToUpdate.Price
                });

                // 5. Получаем конкретный пост (с кэшированием/ETag)
                var specificPost = await _externalService.GetPostAsync(1);
                scenarioResults.Add(new
                {
                    Step = 5,
                    Action = "Get specific post",
                    Post = specificPost
                });

                _logger.LogInformation("End-to-end scenario completed successfully");

                return Ok(new
                {
                    Success = true,
                    Message = "End-to-end scenario completed",
                    Steps = scenarioResults,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in end-to-end scenario");
                return StatusCode(500, new
                {
                    Success = false,
                    Error = ex.Message,
                    CompletedSteps = scenarioResults
                });
            }
        }

        [HttpGet("cached-products")]
        [ResponseCache(Duration = 60)]
        public IActionResult GetCachedProducts()
        {
            var products = _products;
            var responseData = new
            {
                Products = products,
                CachedAt = DateTime.UtcNow,
                Message = "This response is cached for 60 seconds"
            };

            // Генерируем ETag на основе данных
            var content = System.Text.Json.JsonSerializer.Serialize(responseData);
            var etag = GenerateETag(content);

            // Проверяем If-None-Match (правильное сравнение StringValues)
            if (Request.Headers.IfNoneMatch.Count > 0 && 
                Request.Headers.IfNoneMatch[0] == etag)
            {
                return StatusCode(304); // Not Modified
            }

            // Добавляем заголовки только если Response доступен
            if (Response != null)
            {
                Response.Headers.ETag = etag;
                Response.Headers.CacheControl = "public, max-age=60";
            }

            return Ok(responseData);
        }

        [HttpGet("etag-demo")]
        public IActionResult ETagDemo()
        {
            // Статические данные для демонстрации ETag
            var staticData = new 
            { 
                Message = "This is static data for ETag demonstration",
                Version = 1,
                Timestamp = "2024-01-01T00:00:00Z" // Фиксированное время
            };

            var content = System.Text.Json.JsonSerializer.Serialize(staticData);
            var etag = GenerateETag(content);

            // Проверяем If-None-Match
            if (Request.Headers.IfNoneMatch.Count > 0 && 
                Request.Headers.IfNoneMatch[0] == etag)
            {
                return StatusCode(304); // Not Modified
            }

            if (Response != null)
            {
                Response.Headers.ETag = etag;
                Response.Headers.CacheControl = "public, max-age=30";
            }

            return Ok(staticData);
        }

        [HttpGet("simple-cache")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult GetSimpleCache()
        {
            return Ok(new
            {
                Message = "This response is cached on server for 30 seconds",
                Data = _products,
                GeneratedAt = DateTime.UtcNow
            });
        }

        private static string GenerateETag(string content)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(content));
            return $"\"{Convert.ToBase64String(bytes)}\"";
        }
    }
}