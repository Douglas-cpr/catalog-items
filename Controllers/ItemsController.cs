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
    public IEnumerable<ItemDTO> GetItems() 
    {
      var items = _repository.GetItems().Select(item => item.AsDTO());
      return items;
    } 

    [HttpGet("{id}")]
    public ActionResult<ItemDTO> GetItem(Guid id) 
    {
      var item = _repository.GetItem(id);
      if (item is null) return NotFound();
      return Ok(item.AsDTO());
    }

    [HttpPost]
    public ActionResult<ItemDTO> CreateItem(CreateItemDTO itemDTO)
    {
      Item item = new() {
        Id = Guid.NewGuid(),
        Name = itemDTO.Name,
        Price = itemDTO.Price,
        CreatedDate = DateTimeOffset.UtcNow
      };
      _repository.CreateItem(item);
      return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDTO());
    }

    [HttpPut("{id}")]
    public ActionResult UpdateItem(Guid id, UpdateItemDTO itemDTO) 
    {
      var existing = _repository.GetItem(id);
      if (existing is null) return NotFound();
      Item updatedItem = existing with 
      {
        Name = itemDTO.Name,
        Price = itemDTO.Price
      };
      _repository.UpdateItem(updatedItem);
      return NoContent();
    }


    [HttpDelete("{id}")]
    public ActionResult DeleteItem(Guid id)
    {
      var existing = _repository.GetItem(id);
      if (existing is null) return NotFound();
      _repository.DeleteItem(id);
      return NoContent();
    }
  }
}