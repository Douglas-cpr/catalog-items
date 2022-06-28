using Catalog.Api.Dtos;
using Catalog.Api.Api.Entities;

namespace Catalog.Api.Api
{
  public static class Extensions 
  {
    public static ItemDTO AsDTO(this Item item)
    {
      return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }
  }
}