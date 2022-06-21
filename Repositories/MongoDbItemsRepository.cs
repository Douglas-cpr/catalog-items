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

    public async Task CreateItemAsync(Item item)
    {
      await _items.InsertOneAsync(item);
    }

    public async Task DeleteItemAsync(Guid id)
    {
      var filter = _filterBuilder.Eq(item => item.Id, id);
      await _items.DeleteOneAsync(filter);
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
      var filter = _filterBuilder.Eq(item => item.Id, id);
      return await _items.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
      return await _items.Find(new BsonDocument()).ToListAsync();
    }

    public async Task UpdateItemAsync(Item item)
    {
      var filter = _filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
      await _items.ReplaceOneAsync(filter, item);
    }
  }
}