using Task07.Application.Interfaces;
using Task07.Domain.Entities;

namespace Task07.Application.Services;

public class ItemService : IItemService
{
    private static readonly List<Item> _items = new()
    {
        new Item { Id = 1, Name = "Item 1", Description = "Description 1", CreatedAt = DateTime.UtcNow },
        new Item { Id = 2, Name = "Item 2", Description = "Description 2", CreatedAt = DateTime.UtcNow },
        new Item { Id = 3, Name = "Test Item", Description = "Description 3", CreatedAt = DateTime.UtcNow }
    };

    public Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        return Task.FromResult(_items.AsEnumerable());
    }

    public Task<Item?> GetItemByIdAsync(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(item);
    }

    public Task<Item> CreateItemAsync(Item item)
    {
        item.Id = _items.Max(i => i.Id) + 1;
        item.CreatedAt = DateTime.UtcNow;
        _items.Add(item);
        return Task.FromResult(item);
    }
}