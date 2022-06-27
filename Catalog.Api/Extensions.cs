using Catalog.Api.Api.Dtos;
using Catalog.Api.Api.Entities;

namespace Catalog.Api.Api
{
  public static class Extensions 
  {
    public static ItemDTO AsDTO(this Item item)
    {
      return new ItemDTO
      {
        Id = item.Id,
        Name = item.Name,
        Price = item.Price,
        CreatedDate = item.CreatedDate
      };
    }
  }
}