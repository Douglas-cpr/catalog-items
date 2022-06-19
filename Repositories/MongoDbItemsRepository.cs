using Catalog.Entities;
using Catalog.Repositories.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
  public class MongoDbItemsRepository : IItemsRepository
  {
    private const string databaseName = "Catalog";
    private const string collectionName = "Items";
    private readonly IMongoCollection<Item> _items;    

    private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

    public MongoDbItemsRepository(IMongoClient client)
    {
      IMongoDatabase database = client.GetDatabase(databaseName);
      _items = database.GetCollection<Item>(collectionName);
    }

    public void CreateItem(Item item)
    {
      _items.InsertOne(item);
    }

    public void DeleteItem(Guid id)
    {
      var filter = _filterBuilder.Eq(item => item.Id, id);
      _items.DeleteOne(filter);
    }

    public Item GetItem(Guid id)
    {
      var filter = _filterBuilder.Eq(item => item.Id, id);
      return _items.Find(filter).FirstOrDefault();
    }

    public IEnumerable<Item> GetItems()
    {
      return _items.Find(new BsonDocument()).ToList();
    }

    public void UpdateItem(Item item)
    {
      var filter = _filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
      _items.ReplaceOne(filter, item);
    }
  }
}