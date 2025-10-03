namespace Task07.Core.Entities
{
    public class Pagination
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }

    public class PagedList<T>
    {
        public List<T> Items { get; set; } = new();
        public Pagination Pagination { get; set; } = new();
    }
}