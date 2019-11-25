using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Kibin.Models;
namespace Kibin.Services
{
  public class UsersService
  {
      private readonly IMongoCollection<User> _user;
      public UsersService(IMongoDBContext context)
      {
        //   var client = new MongoClient(settings.ConnectionString);
        //   var database = client.GetDatabase(settings.DatabaseName);
          _user = context.Database().GetCollection<User>("Users");
      }
      public List<User> Get() =>
           _user.Find(user => true).ToList();
       public User Get(string id) =>
           _user.Find<User>(user => user.Id == id).FirstOrDefault();
       public User Create(User user)
       {
           _user.InsertOne(user);
           return user;
       }
       public void Update(string id, User userIn) =>
           _user.ReplaceOne(user => user.Id == id, userIn);
       public void Remove(User userIn) =>
           _user.DeleteOne(user => user.Id == userIn.Id);
       public void Remove(string id) =>
           _user.DeleteOne(user => user.Id == id);
  }
  }