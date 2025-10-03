using Task07.Core.Interfaces;
using Task07.Core.Entities;

namespace Task07.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;

        public ItemService(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            item.CreatedAt = DateTime.UtcNow;
            return await _repository.AddAsync(item);
        }

        public async Task<Item?> UpdateItemAsync(int id, Item item)
        {
            var existingItem = await _repository.GetByIdAsync(id);
            if (existingItem == null) return null;

            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.IsCompleted = item.IsCompleted;

            await _repository.UpdateAsync(existingItem);
            return existingItem;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
