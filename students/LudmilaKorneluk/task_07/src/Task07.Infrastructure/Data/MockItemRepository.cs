using Task07.Core.Interfaces;
using Task07.Core.Entities;

namespace Task07.Infrastructure.Data
{
    public class MockItemRepository : IItemRepository
    {
        private readonly List<Item> _items = new();
        private int _nextId = 1;
        
        public MockItemRepository()
        {
            // Тестовые данные
            _items.Add(new Item { 
                Id = _nextId++, 
                Name = "Первая задача", 
                Description = "Описание первой задачи", 
                CreatedAt = DateTime.UtcNow 
            });
        }
        
        public Task<Item?> GetByIdAsync(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            return Task.FromResult(item);
        }
        
        public Task<IEnumerable<Item>> GetAllAsync()
        {
            return Task.FromResult(_items.AsEnumerable());
        }
        
        public Task<Item> AddAsync(Item item)
        {
            item.Id = _nextId++;
            _items.Add(item);
            return Task.FromResult(item);
        }
        
        public Task UpdateAsync(Item item)
        {
            var existingItem = _items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                var index = _items.IndexOf(existingItem);
                _items[index] = item;
            }
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _items.Remove(item);
            }
            return Task.CompletedTask;
        }
    }
}