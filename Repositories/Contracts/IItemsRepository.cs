using System;
using Catalog.Entities;


namespace Catalog.Repositories.Contracts
{
    public interface IItemsRepository
    {
      Item GetItem(Guid id);
      IEnumerable<Item> GetItems();
    }
}
