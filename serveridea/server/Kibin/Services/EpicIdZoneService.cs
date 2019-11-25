using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using Kibin.RabbitMQ;
using MongoDB.Driver;

namespace Kibin.Services
{
    public class EpicIdZoneService : IEpicIdZoneService
    {
        private readonly IMongoCollection<EpicsIdZone> _epic;
        public EpicIdZoneService(IMongoDBContext context)
        {
            //var client = new MongoClient(settings.ConnectionString);
            //var database = client.GetDatabase(settings.DatabaseName);
            _epic = context.Database().GetCollection<EpicsIdZone>("EpicsIdZoneCollection");
        }
        public List<EpicsIdZone> Get() =>
          _epic.Find(epic => true).ToList();
        public EpicsIdZone Get(string id) =>
            _epic.Find<EpicsIdZone>(epic => epic.Id == id).FirstOrDefault();

        public EpicsIdZone Create(EpicsIdZone entity, string username, string userid, string projectId)
        {
            _epic.InsertOne(entity);

            Logger_Domain logD = new Logger_Domain()
            {
                type = "IdeaZone@EpicCreated",
                description = "A member created a Epic",
                published = DateTime.Now.Add(new TimeSpan(0, 5, 30, 0)),
            };
            Logger_Activity logA = new Logger_Activity()
            {
                id = userid,
                type = "Activity@EpicCreated",
                description = username+ " has created an Epic "+ entity.EpicName,
                details = new data()
                {
                    id= entity.Id,
                    name = entity.EpicName
                },
                published = DateTime.Now,
                projectId = projectId
            };
            RabbitMQProducer producerA = new RabbitMQProducer();
            producerA.SendMessageToLoggerA(logA);
            Console.WriteLine(logA);
            RabbitMQProducer producerD = new RabbitMQProducer();
            producerD.SendMessageToLoggerD(logD);
            Console.WriteLine(logD);

            return entity;
        }
        public void Update(string id, EpicsIdZone entity)
        {
            _epic.ReplaceOne(epic => epic.Id == id, entity);

        }
        public void UpdateEpicName(string id, string newName)
        {
            _epic.UpdateOne(
                epic => epic.Id == id,
                Builders<EpicsIdZone>.Update.Set(epic => epic.EpicName, newName));
        }
        public void UpdateEpicStatus(string id, string status,string username, string userid, string projectId)
        {
            Console.WriteLine("status of epic will change to "+ status);
            EpicsIdZone Epic =_epic.Find<EpicsIdZone>(epic => epic.Id == id).FirstOrDefault();

            _epic.UpdateOne(
                epic => epic.Id == id,
                Builders<EpicsIdZone>.Update.Set(epic => epic.status, status));
            Logger_Domain logD = new Logger_Domain()
            {
                
                type = "IdeaZone@UserStoriesMovedWS",          
                description = (status == "requested")?username+ " has requested to move an Epic "+Epic.EpicName+" to Workspace"
                                                     : (status == "productbacklog")?username + " has moved an Epic "+Epic.EpicName + " to ProductBacklog"
                                                     : username + " has moved an Epic " + Epic.EpicName + " Back to Workspace",
                published = DateTime.Now.Add(new TimeSpan(0, 5, 30, 0)),
            };
            Logger_Activity logA = new Logger_Activity()
            {
                id = userid,
                type = "Activity@UserStoryMovedWS",
                description = (status == "requested") ? username + " has requested to move an Epic " + Epic.EpicName + " to Workspace"
                                                      : (status == "productbacklog") ? username + " has moved an Epic " + Epic.EpicName + " to ProductBacklog"
                                                     : username + " has moved an Epic " + Epic.EpicName + " Back to Workspace",
                details = new data()
                {
                    id = id,
                    name = ""
                },
                published = DateTime.Now,
                projectId = projectId
            };
            RabbitMQProducer producerA = new RabbitMQProducer();
            producerA.SendMessageToLoggerA(logA);
            Console.WriteLine(logA);
            RabbitMQProducer producerD = new RabbitMQProducer();
            producerD.SendMessageToLoggerD(logD);
            Console.WriteLine(logD);


        }

        public void UpdateUserStory(string id, string userStory,string username , string story, string userid, string projectId)
        {
            EpicsIdZone Epic = _epic.Find<EpicsIdZone>(epic => epic.Id == id).FirstOrDefault();
            string EpicName = Epic.EpicName;

            _epic.UpdateOne(
             epic => epic.Id == id,
             Builders<EpicsIdZone>.Update.Push<string>(epic => epic.UserStories, userStory));
            Logger_Activity logA = new Logger_Activity()
            {
                id = userid,
                type = "Activity@UserStoryAdded",
                description = username+ " has added an Userstory  "+story+" to Epic "+EpicName,
                details = new data()
                {
                    id = id,
                    name = userStory
                },
                published = DateTime.Now,
                projectId = projectId
            };
            RabbitMQProducer producerA = new RabbitMQProducer();
            producerA.SendMessageToLoggerA(logA);
            Console.WriteLine(logA);
        }
        public void RemoveUserStory(string id,string userStory, string username, string story, string userid, string projectId)
        {
            EpicsIdZone Epic = _epic.Find<EpicsIdZone>(epic => epic.Id == id).FirstOrDefault();
            string EpicName = Epic.EpicName;

            _epic.UpdateOne(
                epic => epic.Id == id,
            Builders<EpicsIdZone>.Update.Pull<string>(epic => epic.UserStories, userStory));
            Logger_Activity logA = new Logger_Activity()
            {
                id = userid,
                type = "Activity@userStoryDeleted",
                description = username + " has removed an Userstory  " + story + " from Epic " + EpicName,
                details = new data()
                {
                    id = id,
                    name = userStory
                },
                published = DateTime.Now,
                projectId = projectId
            };
            RabbitMQProducer producerA = new RabbitMQProducer();
            producerA.SendMessageToLoggerA(logA);
            Console.WriteLine(logA);
        }

        public void Remove(string id, string username, string userid, string projectId)
        {
           EpicsIdZone Epic = _epic.Find<EpicsIdZone>(epic => epic.Id == id).FirstOrDefault();

            Logger_Domain logD = new Logger_Domain()
            {
                type = "IdeaZone@EpicDeleted",
                description = "A member has deleted an Epic",
                published = DateTime.Now.Add(new TimeSpan(0, 5, 30 , 0))
            };
            Logger_Activity logA = new Logger_Activity()
            {
                id = userid,
                type = "Activity@EpicDeleted",
                description = username+" has deleted an Epic "+ Epic.EpicName,
                details = new data()
                {
                    id = id,
                    name = " "
                },
                published = DateTime.Now,
                projectId = projectId


            };
            RabbitMQProducer producerA = new RabbitMQProducer();
            producerA.SendMessageToLoggerA(logA);
            Console.WriteLine(logA);
            RabbitMQProducer producer1 = new RabbitMQProducer();
            producer1.SendMessageToLoggerD(logD);
            Console.WriteLine(logD);
            _epic.DeleteOne(epic => epic.Id == id);
        }

        }



    public interface IEpicIdZoneService
    {
        List<EpicsIdZone> Get();
        EpicsIdZone Get(string id);
        EpicsIdZone Create(EpicsIdZone entity,string username, string userid, string projectId);
        void Update(string id, EpicsIdZone entity);

        void UpdateEpicName(string id, string newname);

        void UpdateEpicStatus(string id, string status, string username, string userid, string projectId);

        void RemoveUserStory(string id, string userStory, string username , string story, string userid, string projectId);

        void UpdateUserStory(string id, string userStory, string username , string story, string userid, string projectId);
        void Remove(string id, string username, string userid, string projectId);
    }
}
