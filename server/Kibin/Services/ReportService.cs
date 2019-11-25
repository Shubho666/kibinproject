using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Kibin.Models;
using System;
using Newtonsoft.Json;
namespace Kibin.Services
{
  public class ReportService
  {
      private readonly IMongoCollection<Report> _report;
      public ReportService(IMongoDBContext context)
      {
         _report = context.Database().GetCollection<Report>("test10");
          
      }
      public List<Report> Get() =>
           _report.Find(report => true).ToList();
       public Report GetbyId(string id) =>
           _report.Find<Report>(report => report.Id == id).FirstOrDefault();

        
        public Report GetByProjectId(string id) =>
           _report.Find<Report>(report => report.project_id == id).FirstOrDefault();

        public Report GetByProjectIdandDate(string id,DateTime date) =>
             
               _report.Find<Report>(report => report.project_id == id ).FirstOrDefault();
           

       
       public Report Create(Report report)
       {  
           _report.InsertOne(report);
           Console.WriteLine("Report:{0}",JsonConvert.SerializeObject(report));
            
       
           return report;
       }
        public UpdateResult Update(string id, List<UserStories> reportIn) => _report.UpdateOne(report => report.project_id == id,
            Builders<Report>.Update.Set("list", reportIn));
          public void UpdateAll(string id, Report userIn) =>
           _report.ReplaceOne(user => user.project_id == id, userIn);
         
           

       public void Remove(Report reportIn) =>
           _report.DeleteOne(report => report.Id == reportIn.Id);
       public void Remove(string id) =>
           _report.DeleteOne(report => report.Id == id);
  }
  }