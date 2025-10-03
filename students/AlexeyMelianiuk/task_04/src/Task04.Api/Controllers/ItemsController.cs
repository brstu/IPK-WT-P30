using Microsoft.AspNetCore.Mvc;

namespace Task04.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private static readonly List<Item> _items = new()
    {
        new Item(1, "Item 1", "Description for item 1"),
        new Item(2, "Item 2", "Description for item 2"),
        new Item(3, "Item 3", "Description for item 3")
    };

    [HttpGet]
    public IActionResult GetItems()
    {
        return Ok(_items);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetItem(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null)
            return NotFound();
        
        return Ok(item);
    }

    [HttpPost]
    public IActionResult CreateItem(Item item)
    {
        item.Id = _items.Max(i => i.Id) + 1;
        _items.Add(item);
        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }
}

public record Item(int Id, string Name, string Description);