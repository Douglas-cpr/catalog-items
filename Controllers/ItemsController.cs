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
  }
}