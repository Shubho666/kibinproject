using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Kibin.Models;
namespace Kibin.Services
{
  public class LoggerService :ILoggerService
  {
      private readonly IMongoCollection<Logger> _logger;
      public LoggerService(IMongoDBContext context)
      {
        //   var client = new MongoClient(settings.ConnectionString);
        //   var database = client.GetDatabase(settings.DatabaseName);
          _logger = context.Database().GetCollection<Logger>("Logger");
      }
      public List<Logger> Get() =>
           _logger.Find(user => true).ToList();
       public Logger Get(string id) =>
           _logger.Find<Logger>(user => user.LoggerId == id).FirstOrDefault();

        public List<Logger> GetByProjectandUser(string projectid,string userid) =>
           _logger.Find<Logger>(user => (user.project_id == projectid && user.id== userid)).ToList();

        public List<Logger> GetByProject(string projectid) =>
           _logger.Find<Logger>(user => user.project_id  == projectid).ToList();
        public Logger Create(Logger user)
       {
           _logger.InsertOne(user);
           return user;
       }
       public void Update(string id, Logger userIn) =>
           _logger.ReplaceOne(user => user.LoggerId == id, userIn);
       public void Remove(Logger userIn) =>
           _logger.DeleteOne(user => user.LoggerId == userIn.LoggerId);
       public void Remove(string id) =>
           _logger.DeleteOne(user => user.LoggerId == id);
  }
    public interface ILoggerService
    {
        List<Logger> Get();
        Logger Get(string id);

        List<Logger> GetByProjectandUser(string projectid, string userid);

        List<Logger> GetByProject(string projectid);
        Logger Create(Logger user);
        void Update(string id, Logger userIn);

        void Remove(Logger userIn);

        void Remove(string id);
    }
  }