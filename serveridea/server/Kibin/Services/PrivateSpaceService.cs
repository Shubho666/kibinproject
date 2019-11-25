using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using MongoDB.Driver;

namespace Kibin.Services
{
    public class PrivateSpaceService: IPrivateSpaceService
    {
        private readonly IMongoCollection<PrivateSpace> _privateSpace;
        public PrivateSpaceService(IMongoDBContext context)
        {
            //var client = new MongoClient(settings.ConnectionString);
            //var database = client.GetDatabase(settings.DatabaseName);
            _privateSpace = context.Database().GetCollection<PrivateSpace>("PrivateSpaceCollection");
        }
        public List<PrivateSpace> Get() =>
           _privateSpace.Find(project => true).ToList();
        public PrivateSpace Get(string id) =>
            _privateSpace.Find<PrivateSpace>(user => user.Id == id).FirstOrDefault();

        public PrivateSpace GetByUserId(string userid) =>
            _privateSpace.Find<PrivateSpace>(user => user.UserId == userid).FirstOrDefault();
        public PrivateSpace Create(PrivateSpace entity)
        {
            _privateSpace.InsertOne(entity);
            return entity;
        }
        public void Updatebyid(string id, PrivateSpace entity)
        {
            _privateSpace.ReplaceOne(user => user.UserId == id, entity);


        }
        public void Update(string id, PrivateSpace entity)
        {
            _privateSpace.ReplaceOne(user => user.Id == id, entity);


        }

        public void Remove(string id) =>
            _privateSpace.DeleteOne(user => user.Id == id);
    }

    public interface IPrivateSpaceService
    {
        List<PrivateSpace> Get();
        PrivateSpace Get(string id);

        PrivateSpace GetByUserId(string UserId);

        PrivateSpace Create(PrivateSpace entity);
        void Update(string UserId, PrivateSpace entity);

        void Updatebyid(string id, PrivateSpace entity);
        void Remove(string id);

    }
}
