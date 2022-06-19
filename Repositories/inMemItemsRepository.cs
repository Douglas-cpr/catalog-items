using Catalog.Entities;

namespace Catalog.Repositories
{
  public class InMemItemsRepository 
  {
    private readonly List<Item> items = new() 
    {
      new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 10, CreatedDate = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 30, CreatedDate = DateTimeOffset.UtcNow }
    };

    public IEnumerable<Item> GetItems => items;

    public Item GetItem(Guid id) => items.FirstOrDefault(item => item.Id == id);
  }
}