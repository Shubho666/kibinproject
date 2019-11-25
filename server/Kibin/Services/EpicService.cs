using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Kibin.Models;

namespace Kibin.Services
{
    public class EpicService
    {
        private readonly IMongoCollection<Epic> _epic;
        public EpicService(IMongoDBContext context)
        {
            _epic = context.Database().GetCollection<Epic>("EpicCollection");
        }
        public List<Epic> Get() =>
             _epic.Find(user => true).ToList();
        public Epic Get(string id) =>
            _epic.Find<Epic>(user => user.Id == id).FirstOrDefault();
        public Epic Create(Epic user)
        {
            _epic.InsertOne(user);
            
            
            return user;
        }
        public void Update(string id, Epic userIn) =>
            _epic.ReplaceOne(user => user.Id == id, userIn);
        public void Remove(Epic userIn) =>
            _epic.DeleteOne(user => user.Id == userIn.Id);
        public void Remove(string id) =>
            _epic.DeleteOne(user => user.Id == id);
    }
}