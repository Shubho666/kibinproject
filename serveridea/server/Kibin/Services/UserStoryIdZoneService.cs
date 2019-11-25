using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using MongoDB.Driver;
using Kibin.RabbitMQ;

namespace Kibin.Services
{
    public class UserStoryIdZoneService : IUserStoryIdZoneService
    {
            private readonly IMongoCollection<UserStoryIdZone> _userStory;
            public UserStoryIdZoneService(IMongoDBContext context)
            {
                //var client = new MongoClient(settings.ConnectionString);
                //var database = client.GetDatabase(settings.DatabaseName);
                _userStory = context.Database().GetCollection<UserStoryIdZone>("UserStoryIdZoneCollection");
            }
            public List<UserStoryIdZone> Get() =>
              _userStory.Find(usstory => true).ToList();
            public UserStoryIdZone Get(string id) =>
                _userStory.Find<UserStoryIdZone>(usstory => usstory.Id == id).FirstOrDefault();

            public UserStoryIdZone Create(UserStoryIdZone entity)
            {
                _userStory.InsertOne(entity);

            Logger_Domain logD = new Logger_Domain()
            {
                type = "IdeaZone@UserStoryCreated",
                description = "A member created a UserStory",
                published = DateTime.Now
                
            };
            Logger_Activity logA = new Logger_Activity()
            {
                id = "user",
                type = "Activity@UserStoryCreated",
                description = "A member Created a Userstory",
                details = new data()
                {
                    id = entity.Id,
                    name = entity.UserStoryName
                },
                published = DateTime.Now,
            };
            RabbitMQProducer producerA = new RabbitMQProducer();
            producerA.SendMessageToLoggerA(logA);
            Console.WriteLine(logA);
            RabbitMQProducer producerD = new RabbitMQProducer();
            producerD.SendMessageToLoggerD(logD);
            Console.WriteLine(logD);

            return entity;
            }
            public void Update(string id, UserStoryIdZone entity)
            {
                _userStory.ReplaceOne(epic => epic.Id == id, entity);

            }
            public void UpdateUserStoryName(string id, string newName)
            {
                _userStory.UpdateOne(
                    usstory => usstory.Id == id,
                    Builders<UserStoryIdZone>.Update.Set(usstory => usstory.UserStoryName, newName));
            }
        public void UpdateUserStoryDescription(string id, string[] newdescription)
        {
            _userStory.UpdateOne(
                usstory => usstory.Id == id,
                Builders<UserStoryIdZone>.Update.Set(usstory => usstory.UserStoryDescription, newdescription));
        }

        public void UpdateStoryType(string id,string type)
        {
            _userStory.UpdateOne(
                usstory => usstory.Id == id,
                Builders<UserStoryIdZone>.Update.Set(usstory => usstory.UserStoryType, type));
        }
        public void UpdateStoryStatus(string id, string status, string ProjectId,string username,string userid)
        {
            Console.WriteLine("ProjectId received while moving user story to Product Backlog " + ProjectId);
            Console.WriteLine(status);
            

            _userStory.UpdateOne(
                usstory => usstory.Id == id,
                Builders<UserStoryIdZone>.Update.Set(usstory => usstory.Status, status));
            UserStoryIdZone obj = _userStory.Find<UserStoryIdZone>(usstory => usstory.Id == id).FirstOrDefault();
            UserStoryIdeas entity = new UserStoryIdeas();
            entity.linkedtoId = obj.Id;
            entity.description = obj.UserStoryName;
            entity.acceptanceCriteria = obj.UserStoryDescription;
            entity.action = "put";
            entity.projectId = ProjectId;
            if (status == "Backlog")
            {
                entity.action = "post";
            }
            else
            {
                entity.action = "delete";
            }
            Console.WriteLine("entity action " + entity.action);

            RabbitMQProducer producerM = new RabbitMQProducer();
            producerM.SendBacklog(entity);
            //producerM.SendBacklogBoard(entity);
            Console.WriteLine(entity);
            Logger_Domain logD = new Logger_Domain()
            {
                type = "IdeaZone@UserStoriesMovedPB",
                description = "A member moved Userstories to Product Backlog",
                published = DateTime.Now.Add(new TimeSpan(0, 5, 30, 0)),
            };
            Logger_Activity logA = new Logger_Activity()
            {
                id = userid,
                type = "Activity@UserStoryMovedPB",
                description = (status == "Backlog") ? username + " moved Userstory ( " + obj.UserStoryName + " ) to Product Backlog"
                                                   : username + " moved Userstory ( " + obj.UserStoryName + " )  back to Workspace",
                details = new data()
                {
                    id = id,
                    name = ""
                },
                published = DateTime.Now,
                projectId = ProjectId
            };
            RabbitMQProducer producerA = new RabbitMQProducer();
            producerA.SendMessageToLoggerA(logA);
            Console.WriteLine(logA);
            RabbitMQProducer producerD = new RabbitMQProducer();
            producerD.SendMessageToLoggerD(logD);
            Console.WriteLine(logD);
        }


        public void Remove(string id)
        {
            _userStory.DeleteOne(usstory => usstory.Id == id);
            Logger_Domain logD = new Logger_Domain()
            {
                type = "IdeaZone@userStoryDeleted",
                description = "A member deleted a UserStory",
                published = DateTime.Now
            };
            Logger_Activity logA = new Logger_Activity()
            {
                id = "user",
                type = "Activity@UserStoryDeleted",
                description = "A member moved Userstories to Product Backlog",
                details = new data()
                {
                    id = id,
                    name = ""
                },
                published = DateTime.Now
            };
            RabbitMQProducer producerA = new RabbitMQProducer();
            producerA.SendMessageToLoggerA(logA);
            Console.WriteLine(logA);
            RabbitMQProducer producerD = new RabbitMQProducer();
            producerD.SendMessageToLoggerD(logD);
            Console.WriteLine(logD);
        }
        }

    public interface IUserStoryIdZoneService
    {
        List<UserStoryIdZone> Get();
        UserStoryIdZone Get(string id);
        UserStoryIdZone Create(UserStoryIdZone entity);
        void Update(string id, UserStoryIdZone entity);
        void UpdateStoryType(string id, string type);

        void UpdateUserStoryName(string id, string newname);

        void UpdateStoryStatus(string id, string status, string ProjectId, string username,string userid);
        void UpdateUserStoryDescription(string id, string[] newDescription);
        void Remove(string id);
    }



}
