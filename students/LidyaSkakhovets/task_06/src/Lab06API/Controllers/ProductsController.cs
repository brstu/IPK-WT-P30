using Microsoft.AspNetCore.Mvc;
using Lab06_API.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Lab06_API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<ProductV1> _productsV1 = new();
        private static List<ProductV2> _productsV2 = new();
        private static int _nextId = 1;

        // Инициализация тестовых данных
        static ProductsController()
        {
            InitializeTestData();
        }

        private static void InitializeTestData()
        {
            // Тестовые данные для v1
            for (int i = 1; i <= 20; i++)
            {
                _productsV1.Add(new ProductV1
                {
                    Id = i,
                    Name = $"Product V1 {i}",
                    Description = $"Description for product {i}",
                    Price = i * 10.5m,
                    Category = i % 2 == 0 ? "Электроника" : "Одежда"
                });
            }

            // Тестовые данные для v2
            for (int i = 1; i <= 20; i++)
            {
                _productsV2.Add(new ProductV2
                {
                    Id = i,
                    Name = $"Product V2 {i}",
                    Description = $"Extended description for product {i}",
                    Price = i * 15.99m,
                    Category = i % 2 == 0 ? "Смартфоны" : "Ноутбуки",
                    Brand = i % 2 == 0 ? "Apple" : "Samsung",
                    StockQuantity = i * 2,
                    Rating = 4.0 + (i % 5 * 0.1)
                });
            }
            _nextId = 21;
        }

        // ========== V1 ENDPOINTS ==========

        [HttpGet]
        [ApiVersion("1.0")]
        [ProducesResponseType(typeof(PagedResponse<ProductV1>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public IActionResult GetProductsV1([FromQuery] PaginationRequest request)
        {
            // Валидация параметров запроса
            if (!TryValidatePaginationRequest(request, out var validationProblem))
            {
                return validationProblem!;
            }

            var query = _productsV1.AsQueryable();

            // Применяем фильтрацию и сортировку
            query = ApplyFiltersAndSorting(query, request);

            // Пагинация
            var response = CreatePagedResponse(query, request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType(typeof(ProductV1), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public IActionResult GetProductV1(int id)
        {
            var product = _productsV1.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return CreateNotFoundProblem(id);
            }

            return Ok(product);
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [ProducesResponseType(typeof(ProductV1), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public IActionResult CreateProductV1([FromBody] ProductV1 product)
        {
            if (!ModelState.IsValid)
            {
                return CreateValidationProblem();
            }

            product.Id = _nextId++;
            _productsV1.Add(product);
            return CreatedAtAction(nameof(GetProductV1), new { id = product.Id, version = "1.0" }, product);
        }

        // ========== V2 ENDPOINTS ==========

        [HttpGet]
        [ApiVersion("2.0")]
        [ProducesResponseType(typeof(PagedResponse<ProductV2>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public IActionResult GetProductsV2([FromQuery] PaginationRequest request)
        {
            // Валидация параметров запроса
            if (!TryValidatePaginationRequest(request, out var validationProblem))
            {
                return validationProblem!;
            }

            var query = _productsV2.AsQueryable();

            // Применяем фильтрацию и сортировку
            query = ApplyFiltersAndSortingV2(query, request);

            // Пагинация
            var response = CreatePagedResponse(query, request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ApiVersion("2.0")]
        [ProducesResponseType(typeof(ProductV2), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public IActionResult GetProductV2(int id)
        {
            var product = _productsV2.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return CreateNotFoundProblem(id);
            }

            return Ok(product);
        }

        [HttpPost]
        [ApiVersion("2.0")]
        [ProducesResponseType(typeof(ProductV2), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public IActionResult CreateProductV2([FromBody] ProductV2 product)
        {
            if (!ModelState.IsValid)
            {
                return CreateValidationProblem();
            }

            product.Id = _nextId++;
            _productsV2.Add(product);
            return CreatedAtAction(nameof(GetProductV2), new { id = product.Id, version = "2.0" }, product);
        }

        [HttpPut("{id}")]
        [ApiVersion("2.0")]
        [ProducesResponseType(typeof(ProductV2), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public IActionResult UpdateProductV2(int id, [FromBody] ProductV2 product)
        {
            if (!ModelState.IsValid)
            {
                return CreateValidationProblem();
            }

            var existing = _productsV2.FirstOrDefault(p => p.Id == id);
            if (existing == null)
            {
                return CreateNotFoundProblem(id);
            }

            // Обновляем поля
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Category = product.Category;
            existing.Brand = product.Brand;
            existing.StockQuantity = product.StockQuantity;
            existing.Rating = product.Rating;

            return Ok(existing);
        }

        // ========== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ==========

        private bool TryValidatePaginationRequest(PaginationRequest request, out IActionResult? problemResult)
        {
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

            if (!isValid)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Ошибка валидации параметров запроса",
                    Detail = "Неверные параметры пагинации или фильтрации",
                    Status = 400,
                    Instance = HttpContext.Request.Path
                };
                
                foreach (var validationResult in validationResults)
                {
                    problemDetails.Extensions.Add(validationResult.MemberNames.FirstOrDefault() ?? "request", validationResult.ErrorMessage);
                }

                problemResult = BadRequest(problemDetails);
                return false;
            }

            problemResult = null;
            return true;
        }

        private IQueryable<ProductV1> ApplyFiltersAndSorting(IQueryable<ProductV1> query, PaginationRequest request)
        {
            // Фильтрация
            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(p => p.Name.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                                       (p.Description != null && p.Description.Contains(request.Filter, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                query = query.Where(p => p.Category.Equals(request.Category, StringComparison.OrdinalIgnoreCase));
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            // Сортировка
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                query = request.SortBy.ToLower() switch
                {
                    "name" => request.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Name) 
                        : query.OrderBy(p => p.Name),
                    "price" => request.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Price) 
                        : query.OrderBy(p => p.Price),
                    "category" => request.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Category) 
                        : query.OrderBy(p => p.Category),
                    _ => query.OrderBy(p => p.Id)
                };
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            return query;
        }

        private IQueryable<ProductV2> ApplyFiltersAndSortingV2(IQueryable<ProductV2> query, PaginationRequest request)
        {
            // Фильтрация
            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(p => p.Name.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                                       (p.Description != null && p.Description.Contains(request.Filter, StringComparison.OrdinalIgnoreCase)) ||
                                       p.Brand.Contains(request.Filter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                query = query.Where(p => p.Category.Equals(request.Category, StringComparison.OrdinalIgnoreCase));
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            // Сортировка
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                query = request.SortBy.ToLower() switch
                {
                    "name" => request.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Name) 
                        : query.OrderBy(p => p.Name),
                    "price" => request.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Price) 
                        : query.OrderBy(p => p.Price),
                    "category" => request.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Category) 
                        : query.OrderBy(p => p.Category),
                    "brand" => request.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Brand) 
                        : query.OrderBy(p => p.Brand),
                    "rating" => request.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Rating) 
                        : query.OrderBy(p => p.Rating),
                    _ => query.OrderBy(p => p.Id)
                };
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            return query;
        }

        private PagedResponse<T> CreatePagedResponse<T>(IQueryable<T> query, PaginationRequest request)
        {
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var data = query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResponse<T>
            {
                Data = data,
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNext = request.Page < totalPages,
                HasPrevious = request.Page > 1
            };
        }

        private IActionResult CreateNotFoundProblem(int id)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Продукт не найден",
                Detail = $"Продукт с ID {id} не существует",
                Status = 404,
                Instance = HttpContext.Request.Path
            });
        }

        private IActionResult CreateValidationProblem()
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Ошибка валидации",
                Detail = "Неверные данные в запросе",
                Status = 400,
                Instance = HttpContext.Request.Path
            };

            foreach (var error in ModelState)
            {
                if (error.Value.Errors.Count > 0)
                {
                    problemDetails.Extensions.Add(error.Key, error.Value.Errors.Select(e => e.ErrorMessage));
                }
            }

            return BadRequest(problemDetails);
        }
    }
}