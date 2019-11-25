using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using MongoDB.Driver;

namespace Kibin.Services
{
    public class LinkService
    {
        private readonly IMongoCollection<Link> _user;
        public LinkService(IMongoDBContext context)
        {
            // var client = new MongoClient(settings.ConnectionString);
            // var database = client.GetDatabase(settings.DatabaseName);
            _user = context.Database().GetCollection<Link>("Link");
        }
        public List<Link> Get() =>
             _user.Find(user => true).ToList();
        public Link Get(string id) =>
            _user.Find<Link>(user => user.LinkId == id).FirstOrDefault();
        public Link Create(Link user)
        {
            _user.InsertOne(user);
            return user;
        }
        public void Update(string id, Link userIn) =>
            _user.ReplaceOne(user => user.LinkId == id, userIn);
        public void Remove(Link userIn) =>
            _user.DeleteOne(user => user.LinkId == userIn.LinkId);
        public void Remove(string id) =>
            _user.DeleteOne(user => user.LinkId == id);

        public List<Link> GetByProjectId(string id) =>
        _user.Find<Link>(user => user.project_id == id).ToList();
         public Link GetByGanttId(long id) =>
        _user.Find<Link>(user => user.id == id).FirstOrDefault();

        public void RemoveByGanttId(long id)=>
         _user.DeleteOne(user => user.id == id);

         public UpdateResult PutByGanttId(long id,Link userIn){
             return _user.UpdateOne(
                user =>user.id==id,
                Builders<Link>.Update.Set("source",userIn.source)
                .Set("target",userIn.target)
                .Set("type",userIn.type)

             );
         }
    }
}
