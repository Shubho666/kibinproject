using System;
using System.Collections.Generic;
using System.Linq;
using Kibin.Models;
using MongoDB.Driver;
using Kibin.SignalR_Hub;
namespace Kibin.Services
{
    public class KanbanUSService : IKanbanUSService
    {
        private readonly IMongoCollection<KanbanUserStory> _user;
        public KanbanUSService(IMongoDBContext context)
        {

            _user = context.Database().GetCollection<KanbanUserStory>("userStoryKanban5");
        }
        public List<KanbanUserStory> Get() =>
             _user.Find(user => true).ToList();
        public KanbanUserStory Get(string id) =>
            _user.Find<KanbanUserStory>(user => user.Id == id).FirstOrDefault();

        public KanbanUserStory GetByUniqueId(string id) =>
           _user.Find<KanbanUserStory>(user => user.uniqueId == id).FirstOrDefault();

        public List<KanbanUserStory> GetLastUserId(string userid) =>
            _user.Find(user => user.shortName == userid).ToList();
        public KanbanUserStory GetUserId(string userid) =>
        _user.Find<KanbanUserStory>(user => user.userId == userid).FirstOrDefault();

        public List<KanbanUserStory> GetByProjectId(string userid) =>
            _user.Find<KanbanUserStory>(user => user.projectId == userid).ToList();

        public List<KanbanUserStory> GetByLinkedId(string userid) =>
            _user.Find<KanbanUserStory>(user => user.linkedToId == userid).ToList();



        public KanbanUserStory Create(KanbanUserStory user)
        {
            _user.InsertOne(user);
            return user;
        }
        public void Update(string id, KanbanUserStory userIn) =>
            _user.ReplaceOne(user => user.Id == id, userIn);
        public void Remove(KanbanUserStory userIn) =>
            _user.DeleteOne(user => user.Id == userIn.Id);
        public void Remove(string id) =>
            _user.DeleteOne(user => user.Id == id);
        public void RemoveByUniqueId(string id) =>
        _user.DeleteOne(user => user.uniqueId == id);




        //RabbitMQSignalR

        // public void SendRabbit(string projectid)
        // {
        //     ChatHub b=new ChatHub();
        //     b.SendUserStoryToAddOnList(projectid,"suc");
        // }


    }

    public interface IKanbanUSService
    {
        List<KanbanUserStory> Get();
        KanbanUserStory Get(string id);

        KanbanUserStory Create(KanbanUserStory entity);
        void Update(string id, KanbanUserStory name);
        void Remove(string id);
        List<KanbanUserStory> GetLastUserId(string id);
        KanbanUserStory GetUserId(string id);
        List<KanbanUserStory> GetByProjectId(string id);
        List<KanbanUserStory> GetByLinkedId(string id);
        KanbanUserStory GetByUniqueId(string id);
    }
}