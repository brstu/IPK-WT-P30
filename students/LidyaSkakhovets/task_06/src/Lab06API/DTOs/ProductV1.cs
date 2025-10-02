using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Lab06_API.DTOs
{
    public class ProductV1
    {
        [SwaggerSchema("ID продукта")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название продукта обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        [SwaggerSchema("Название продукта", Nullable = false)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        [SwaggerSchema("Описание продукта")]
        public string? Description { get; set; }

        [Range(0.01, 100000, ErrorMessage = "Цена должна быть между 0.01 и 100000")]
        [SwaggerSchema("Цена продукта")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Категория обязательна")]
        [SwaggerSchema("Категория продукта", Nullable = false)]
        public string Category { get; set; } = string.Empty;
    }
}