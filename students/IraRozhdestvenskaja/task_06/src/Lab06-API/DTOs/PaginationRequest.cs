using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Lab06_API.DTOs
{
    public class PaginationRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Номер страницы должен быть положительным")]
        [SwaggerSchema("Номер страницы", Description = "Номер страницы начиная с 1")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Размер страницы должен быть от 1 до 100")]
        [SwaggerSchema("Размер страницы", Description = "Количество элементов на странице")]
        public int PageSize { get; set; } = 10;

        [SwaggerSchema("Поле для сортировки", Description = "Поле для сортировки (name, price, category)")]
        public string? SortBy { get; set; }

        [SwaggerSchema("Направление сортировки", Description = "Направление сортировки (asc, desc)")]
        public string? SortOrder { get; set; } = "asc";

        [SwaggerSchema("Фильтр по названию", Description = "Фильтр по названию или описанию")]
        public string? Filter { get; set; }

        [SwaggerSchema("Фильтр по категории", Description = "Фильтр по категории")]
        public string? Category { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Минимальная цена не может быть отрицательной")]
        [SwaggerSchema("Минимальная цена", Description = "Минимальная цена для фильтрации")]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Максимальная цена не может быть отрицательной")]
        [SwaggerSchema("Максимальная цена", Description = "Максимальная цена для фильтрации")]
        public decimal? MaxPrice { get; set; }
    }
}