using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Kibin.Models;
using Kibin.RabbitMQ;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Kibin.Services
{
    public class ListService
    {
        private readonly IMongoCollection<List> _list;

        
        public ListService(IMongoDBContext context)
        {   
            _list = context.Database().GetCollection<List>("ListCollections2");
        }

        public List<List> Get() =>
            _list.Find(user => true).ToList();
        public List Get(string id) =>
            _list.Find<List>(user => user.Id == id).FirstOrDefault();
        public List GetName(string name) =>
            _list.Find<List>(user => user.name == name).FirstOrDefault();
        public List<List> GetProjectById(string id) =>
            _list.Find(user => user.projectId == id).ToList();
        public List GetByProjectIdAndPB(string id)=>
        _list.Find(user=>((user.projectId==id)&&(user.name=="Product Backlog"))).FirstOrDefault();
        // public List GetProjectById(string id) =>
        //     _list.Find(user => user.projectId == id).FirstOrDefault();
        public List Create(List user)
        {
            _list.InsertOne(user);
            // Logger log = new Logger()
            // {
            //     type = "KanBan@CreateList",
            //     id = actorId,
            //     description = "List created",
            //     published = DateTime.Now,
            //     data = new Data()
            //     {
            //         id = user.Id,
            //         name = user.name
            //     }
            // };
            // RabbitMQProducer producer = new RabbitMQProducer();
            // producer.SendMessageToLogger(log);
            // Console.WriteLine(log);
     
           
            // Console.Write("Testitng",reports[0].project_id);

            


            

           return user;
        }
        public void Update(string id, List userIn)
        {
            _list.ReplaceOne(user => user.Id == id, userIn);
            
            // Logger log = new Logger()
            // {
            //     type = "KanBan@UpdateList",
            //     id = actorId,
            //     description = "List updated",
            //     published = DateTime.Now,
            //     data = new Data()
            //     {
            //         id = userIn.Id,
            //         name = userIn.name
            //     }
            // };
            // RabbitMQProducer producer = new RabbitMQProducer();
            // producer.SendMessageToLogger(log);
            // Console.WriteLine(log);
            
        }
        public void Remove(List userIn) =>
            _list.DeleteOne(user => user.Id == userIn.Id);
        public void Remove(string id)
        {
            _list.DeleteOne(user => user.Id == id);
            // Logger log = new Logger()
            // {
            //     type = "KanBan@DeleteList",
            //     id = actorId,
            //     description = "List deleted",
            //     published = DateTime.Now,
            //     data = new Data()
            //     {
            //         id = id,
            //         name = Get(id).name
            //     }
            // };
            // RabbitMQProducer producer = new RabbitMQProducer();
            // producer.SendMessageToLogger(log);
            // Console.WriteLine(log);
        }

    }
}