using Microsoft.AspNetCore.Mvc;
using Task07.Core.Entities;
using Task07.Core.Interfaces;

namespace Task07.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        
        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Get()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        
        [HttpPost]
        public async Task<ActionResult<Item>> Post(Item item)
        {
            var createdItem = await _itemService.CreateItemAsync(item);
            return CreatedAtAction(nameof(Get), new { id = createdItem.Id }, createdItem);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<Item>> Put(int id, Item item)
        {
            var updatedItem = await _itemService.UpdateItemAsync(id, item);
            if (updatedItem == null) return NotFound();
            return Ok(updatedItem);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _itemService.DeleteItemAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}