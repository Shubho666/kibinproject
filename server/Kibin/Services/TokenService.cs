using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Kibin.Models;
namespace Kibin.Services
{
  public class TokenService
  {
      private readonly IMongoCollection<Token> _token;
      public TokenService(IMongoDBContext context)
      {
          _token = context.Database().GetCollection<Token>("Token");
          
      }
      public List<Token> Get() =>
           _token.Find(user => true).ToList();
       public Token Get(string id) =>
           _token.Find<Token>(user => user.Id == id).FirstOrDefault();

      public List<Token> GetByUserId(string id) =>
           _token.Find<Token>(user => user.userId == id).ToList();

        
      
       public Token Create(Token user)
       {
           _token.InsertOne(user);
           return user;
       }
       public void Update(string id, Token userIn) =>
           _token.ReplaceOne(user => user.Id == id, userIn);

       
       public void Remove(string id) =>
           _token.DeleteOne(user => user.Id == id);
  }
}