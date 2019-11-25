using System;
using MongoDB.Driver;
namespace Kibin.Models
{
  public class MongoDBContext : IMongoDBContext
  {
      private IMongoDatabase _database;
      public MongoDBContext() {
          var client = new MongoClient(Environment.GetEnvironmentVariable("DBConnectionString"));
          _database = client.GetDatabase(Environment.GetEnvironmentVariable("DBName"));
      }
      public IMongoDatabase Database() {
          return _database;
      }
  }
  public interface IMongoDBContext
  {
      IMongoDatabase Database();
  }
}