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
         _user = context.Database().GetCollection<User>("Users");
          
      }
      public List<User> Get() =>
           _user.Find(user => true).ToList();
       public User Get(string id) =>
           _user.Find<User>(user => user.Id == id).FirstOrDefault();

        
        public User GetByUsername(string name) =>
           _user.Find<User>(user => user.username == name).FirstOrDefault();

        public string GetAccessToken(string id){
            return _user.Find<User>(user=>user.Id==id).FirstOrDefault().accessToken;
        }
       public User Create(User user)
       {
           _user.InsertOne(user);
           return user;
       }
       public void Update(string id, User userIn) =>
           _user.ReplaceOne(user => user.Id == id, userIn);

        public void UpdateAccessToken(string id,string token){
            _user.FindOneAndUpdate(
                user=>user.Id==id,
                Builders<User>.Update.Set("accessToken",token)
            );
        }

        public void UpdateJwt(string id,string token){
            _user.FindOneAndUpdate(
                user=>user.Id==id,
                Builders<User>.Update.Set("accessTokenForSwagge",token)
            );
        }
       public void Remove(User userIn) =>
           _user.DeleteOne(user => user.Id == userIn.Id);
       public void Remove(string id) =>
           _user.DeleteOne(user => user.Id == id);
  }
  }