namespace Task07.Infrastructure.Data
{
    public static class MockData
    {
        public static List<Item> GetItems()
        {
            return new List<Item>
            {
                new Item { Id = 1, Name = "Item 1" },
                new Item { Id = 2, Name = "Item 2" }
                // Добавьте тестовые данные
            };
        }
    }
}