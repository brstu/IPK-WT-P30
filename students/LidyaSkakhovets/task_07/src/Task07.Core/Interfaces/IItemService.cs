using Task07.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task07.Core.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(int id);
        Task<Item> CreateItemAsync(Item item);
        Task<Item?> UpdateItemAsync(int id, Item item);
        Task<bool> DeleteItemAsync(int id);
    }
}