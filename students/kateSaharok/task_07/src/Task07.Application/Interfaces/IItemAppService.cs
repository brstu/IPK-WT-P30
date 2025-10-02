using Task07.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task07.Application.Interfaces
{
    public interface IItemAppService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<ItemDto> CreateItemAsync(CreateItemDto createItemDto);
        Task<ItemDto?> UpdateItemAsync(int id, CreateItemDto updateItemDto);
        Task<bool> DeleteItemAsync(int id);
    }
}