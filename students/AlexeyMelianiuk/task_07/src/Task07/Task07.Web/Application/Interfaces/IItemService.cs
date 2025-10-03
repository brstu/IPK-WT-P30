using Task07.Domain.Entities;

namespace Task07.Application.Interfaces;

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllItemsAsync();
    Task<Item?> GetItemByIdAsync(int id);
    Task<Item> CreateItemAsync(Item item);
}