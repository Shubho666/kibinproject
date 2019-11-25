using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using MongoDB.Driver;

namespace Kibin.Services
{
    public class WorkSpaceService : IWorkSpaceService
    {

        private readonly IMongoCollection<WorkSpace> _workspace;
        public WorkSpaceService(IMongoDBContext context)
        {
            //var client = new MongoClient(settings.ConnectionString);
            //var database = client.GetDatabase(settings.DatabaseName);
            _workspace = context.Database().GetCollection<WorkSpace>("WorkSpaceCollection");
        }

        public List<WorkSpace> Get() =>
           _workspace.Find(project => true).ToList();
        public WorkSpace Get(string id) =>
            _workspace.Find<WorkSpace>(project => project.Id == id).FirstOrDefault();

        public WorkSpace GetByProjectId(string ProjectId) =>
              _workspace.Find<WorkSpace>(project => project.ProjectId == ProjectId).FirstOrDefault();

        public WorkSpace Create(WorkSpace entity)
        {
            _workspace.InsertOne(entity);
            return entity;
        }
        public void Update(string id, string  epicid)
        {
            _workspace.UpdateOne(
              project => project.ProjectId == id,
              Builders<WorkSpace>.Update.Push<string>(project => project.epics, epicid));

        }

        public void DeleteEpic(string id, string epicid)
        {
            _workspace.UpdateOne(
                project => project.ProjectId == id,
                Builders<WorkSpace>.Update.Pull<string>(project => project.epics, epicid));
                
        }

        public void Remove(string  id) =>
            _workspace.DeleteOne(project => project.Id == id);

    }
    public interface IWorkSpaceService
    {
        List<WorkSpace> Get();
        WorkSpace Get(string id);
        WorkSpace GetByProjectId(string ProjectId);
        WorkSpace Create(WorkSpace entity);
        void Update(string id,string epicid);

        void DeleteEpic(string id, string epicid);
        void Remove(string id);

    }
}
