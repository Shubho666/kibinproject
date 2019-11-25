using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Kibin.Models;
using Kibin.RabbitMQ;

namespace Kibin.Services
{
    public class LoggerActivityService
    {
        private readonly IMongoCollection<LoggerActivity> _logger;
        public LoggerActivityService(IMongoDBContext context)
        {
            _logger = context.Database().GetCollection<LoggerActivity>("LoggerActivityCollection1");
        }
        public List<LoggerActivity> Get() =>
            _logger.Find(user => true).ToList();

        public LoggerActivity Get(string id) =>
            _logger.Find<LoggerActivity>(user => user.Id == id).FirstOrDefault();

        public List<LoggerActivity> GetByUserId(string id) =>
            _logger.Find(user => user.userid == id).ToList();

        public List<LoggerActivity> GetByProjectId(string id) =>
            _logger.Find(user => user.projectId == id).ToList();

        public List<LoggerActivity> GetByProjectAndUser(string usid, string projectid) =>
            _logger.Find(user => ((user.projectId == projectid) && (user.userid == usid))).ToList();

        public LoggerActivity Create(LoggerActivity user)
        {
            _logger.InsertOne(user);
            return user;
        }

        public void Update(string id, LoggerActivity userIn) =>
            _logger.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(LoggerActivity userIn) =>
            _logger.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _logger.DeleteOne(user => user.Id == id);
        public void RemoveAll(string id) =>
        _logger.DeleteMany(user => user.projectId == id);
    }
}