using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
  [ApiController]
  [Route("items")]
  public class ItemsController :  ControllerBase
  {
    private readonly IItemsRepository _repository;

    public ItemsController(IItemsRepository repository)
    {
      _repository = repository;
    }
 
    [HttpGet]
    public async Task<IEnumerable<ItemDTO>> GetItemsAsync() 
    {
      var items = (await _repository.GetItemsAsync())
                  .Select(item => item.AsDTO());
      return items;
    } 

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDTO>> GetItemAsync(Guid id) 
    {
      var item = await _repository.GetItemAsync(id);
      if (item is null) return NotFound();
      return Ok(item.AsDTO());
    }

    [HttpPost]
    public async Task<ActionResult<ItemDTO>> CreateItemAsync(CreateItemDTO itemDTO)
    {
      Item item = new() {
        Id = Guid.NewGuid(),
        Name = itemDTO.Name,
        Price = itemDTO.Price,
        CreatedDate = DateTimeOffset.UtcNow
      };
      await _repository.CreateItemAsync(item);
      return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDTO());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDTO itemDTO) 
    {
      var existing = await _repository.GetItemAsync(id);
      if (existing is null) return NotFound();
      Item updatedItem = existing with 
      {
        Name = itemDTO.Name,
        Price = itemDTO.Price
      };
      await _repository.UpdateItemAsync(updatedItem);
      return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItemAsync(Guid id)
    {
      var existing = await _repository.GetItemAsync(id);
      if (existing is null) return NotFound();
      await _repository.DeleteItemAsync(id);
      return NoContent();
    }
  }
}