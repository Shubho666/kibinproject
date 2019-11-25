using System;
using System.Collections.Generic;
using System.Text;
using Kibin.Models;
using Kibin.Services;
using MongoDB.Driver;
using System.Linq;

namespace Kibin.Tests
{
    class TaskServiceTest : ITaskService
    {

        private List<Tasks> _tasks;
        public TaskServiceTest()
        {
            _tasks = new List<Tasks>()
            {
                new Tasks(){TaskId="abcde", id=345678345, action="post", unique_id="fghij", project_id="klmno", start_date=2345674, end_date=123456789, text="Testing123", progress=0.2, duration=3},
                new Tasks(){TaskId="pqrst", id=876543212, action="put", unique_id="uvwxy", project_id="zabcd", start_date=987654, end_date=987654, text="Testing456", progress=0.4, duration=6},
            };
        }

        public List<Tasks> Get()
        {
            return _tasks;
        }
        public Tasks Get(string id)
        {
            return _tasks[0];
        }
        public Tasks GetByGanttMadeId(long id)
        {
            return _tasks[0];
        }
        public Tasks GetByUniqueId(string id)
        {
            return _tasks[0];
        }
        public Tasks GetByGanttId(long id)
        {
            return _tasks[0];
        }

        public List<Tasks> GetByProjectId(string id)
        {
            return _tasks;
        }
        public Tasks Create(Tasks entity)
        {
            _tasks.Add(entity);
            return entity;
        }

        public void Update(string id, Tasks userIn)
        {
            // _tasks.ReplaceOne(entity => entity.Id ==id,userIn);
            throw new NotImplementedException();
        }
        public void Remove(Tasks userIn)
        {
            // _tasks.DeleteOne(user => user.Id == userIn.id);
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            // _tasks.DeleteOne(user => user.Id == userIn.id);
            throw new NotImplementedException();
        }


        public void RemoveByGanttId(long id)
        {
            //_tasks.DeleteOne(user => user.Id == userIn.id);
            throw new NotImplementedException();
        }

        public void UpdateByGanttId(long id, Tasks userIn)
        {
            throw new NotImplementedException();
        }

        public UpdateResult PutByGanttId(long id, Tasks1 userIn)
        {
            throw new NotImplementedException();
        }

        public UpdateResult PutByUniqueId(string id, Tasks userIn)
        {
            throw new NotImplementedException();
        }

        public void RemoveByUniqueId(string id)
        {
            throw new NotImplementedException();
        }

    }
}

