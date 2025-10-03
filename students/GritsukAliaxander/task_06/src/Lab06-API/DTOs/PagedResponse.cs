using Swashbuckle.AspNetCore.Annotations;

namespace Lab06_API.DTOs
{
    public class PagedResponse<T>
    {
        [SwaggerSchema("Данные страницы")]
        public List<T> Data { get; set; } = new();

        [SwaggerSchema("Текущая страница")]
        public int CurrentPage { get; set; }

        [SwaggerSchema("Размер страницы")]
        public int PageSize { get; set; }

        [SwaggerSchema("Всего элементов")]
        public int TotalCount { get; set; }

        [SwaggerSchema("Всего страниц")]
        public int TotalPages { get; set; }

        [SwaggerSchema("Есть следующая страница")]
        public bool HasNext { get; set; }

        [SwaggerSchema("Есть предыдущая страница")]
        public bool HasPrevious { get; set; }
    }
}