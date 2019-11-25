using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using MongoDB.Driver;
using Kibin.RabbitMQ;

namespace Kibin.Services
{
    public class TaskService :ITaskService
    {
        private readonly IMongoCollection<Tasks> _user;
        public TaskService(IMongoDBContext context)
        {

            _user = context.Database().GetCollection<Tasks>("Tasks");
        }
        public Tasks GetByGanttMadeId(long id) =>

        _user.Find<Tasks>(user => user.id == id).FirstOrDefault();

        public List<Tasks> Get() =>
             _user.Find(user => true).ToList();
        public Tasks Get(string id) =>
            _user.Find<Tasks>(user => user.TaskId == id).FirstOrDefault();
        public Tasks GetByUniqueId(string id) =>

        _user.Find<Tasks>(user => user.unique_id == id).FirstOrDefault();

        public Tasks Create(Tasks user)
        {
            _user.InsertOne(user);
            Console.WriteLine("in Service created user{0}" , user);

            return user;
        }

        public void Update(string id, Tasks userIn) =>
            _user.ReplaceOne(user => user.TaskId == id, userIn);
        public void Remove(Tasks userIn) =>
            _user.DeleteOne(user => user.TaskId == userIn.TaskId);
        public void Remove(string id) =>
            _user.DeleteOne(user => user.TaskId == id);

        public Tasks GetByGanttId(long id) =>
        _user.Find<Tasks>(user => user.id == id).FirstOrDefault();

        public List<Tasks> GetByProjectId(string id) =>
        _user.Find<Tasks>(user => user.project_id == id).ToList();

        public void RemoveByGanttId(long id) =>
            _user.DeleteOne(user => user.id == id);

        public void UpdateByGanttId(long id, Tasks userIn) =>
           _user.ReplaceOne(user => user.id == id, userIn);

        public UpdateResult PutByGanttId(long id, Tasks1 userIn)
        {
            return _user.UpdateOne(
                user => user.id == id,
                Builders<Tasks>.Update.Set("duration", userIn.duration)
                 .Set("start_date", userIn.start_date)
                 .Set("progress", userIn.progress)
                 .Set("text", userIn.text)
                 .Set("end_date", userIn.end_date)
           );
        }

        public UpdateResult PutByUniqueId(string id, Tasks userIn)
        {
            return _user.UpdateOne(
                user => user.TaskId == id,
                Builders<Tasks>.Update.Set("duration", userIn.duration)
                 .Set("start_date", userIn.start_date)
                 .Set("progress", userIn.progress)
                 .Set("text", userIn.text)
                 .Set("end_date", userIn.end_date)
           );
        }

        public void RemoveByUniqueId(string id) =>
            _user.DeleteOne(user => user.unique_id == id);
    }

    public interface ITaskService
    {
        Tasks GetByGanttMadeId(long id);
        List<Tasks> Get();
        Tasks Get(string id);
        Tasks GetByUniqueId(string id);

        Tasks Create(Tasks user);

        void Update(string id, Tasks userIn);
        void Remove(Tasks userIn);

        void Remove(string id);

        Tasks GetByGanttId(long id);

        List<Tasks> GetByProjectId(string id);

        void RemoveByGanttId(long id);

        void UpdateByGanttId(long id, Tasks userIn);

        UpdateResult PutByGanttId(long id, Tasks1 userIn);

        UpdateResult PutByUniqueId(string id, Tasks userIn);
        void RemoveByUniqueId(string id);

    }
}
