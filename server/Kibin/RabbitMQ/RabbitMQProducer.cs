using System;
using System.Collections.Generic;
using System.Text;
using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
namespace Kibin.RabbitMQ {
    public class RabbitMQProducer {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        static MongoDBContext ctx = new MongoDBContext ();
        // var UsersService = new UsersService (ctx);
        static LoggerActivityService loggerActivityService = new LoggerActivityService (ctx);
        //static ListService listService = new ListService(ctx);

        private const string BoardsExchangeName = "Boards_Exchange";
        private const string BoardsQueueName = "BoardsToGantt";
        private const string BoardsQueueName1 = "Boards";

        public RabbitMQProducer () {
            CreateConnection ();
        }

        private static void CreateConnection () {
            _factory = new ConnectionFactory {
                HostName = "rabbitmq", UserName = "guest", Password = "guest"
            };

            _connection = _factory.CreateConnection ();
            _model = _connection.CreateModel ();
            _model.ExchangeDeclare (BoardsExchangeName, "topic", durable : true);

            _model.QueueDeclare (BoardsQueueName, true, false, false, null);
            _model.QueueBind (BoardsQueueName, BoardsExchangeName, "boardstogantt");

            _model.QueueDeclare (BoardsQueueName1, true, false, false, null);
            _model.QueueBind (BoardsQueueName1, BoardsExchangeName, "Kibin");

        }

        public void Close () {
            _connection.Close ();
        }

        // public void SendBacklog(KanbanUserStory userstory)
        // {   
        //    // UserStoryIdeas messageforGantt=new UserStoryIdeas() ;
        //    // messageforGantt= ConvertforGantt(messageforGantt,userstory);
        //     byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(userstory));
        //     SendMessage(messagebuffer, "Kibin");
        //     Console.WriteLine(JsonConvert.SerializeObject(userstory));
        // }
        // public void SendMessage(byte[] message, string routingKey)
        // {
        //     _model.BasicPublish(BoardsExchangeName, routingKey, null, message);//routing key to send to a particular queue
        // }

        public void ProduceMessageToGantt (KanbanData kanbanus) {
            Console.WriteLine (JsonConvert.SerializeObject (kanbanus));
            byte[] messagebuffer = Encoding.Default.GetBytes (JsonConvert.SerializeObject (kanbanus));
            //SendMessage(messagebuffer, "Kibin");

            _model.BasicPublish (BoardsExchangeName, "boardstogantt", null, messagebuffer); // message tp gantt

        }

        public void SendMessageToLogger (Logger log) {
            Console.WriteLine (JsonConvert.SerializeObject (log));
            byte[] messagebuffer = Encoding.Default.GetBytes (JsonConvert.SerializeObject (log));
            //_model.BasicPublish(BoardsExchangeName, "Kibin",null,messagebuffer);
            _model.BasicPublish (BoardsExchangeName, "Kibin", null, messagebuffer);
        }
        public void SendMessageToActivityLogger (Logger log) {
            LoggerActivity loggeract = new LoggerActivity ();
            //     public string type{get;set;}

            // public string userid{get;set;}    //    UserId
            // public DateTime published{get;set;}
            // public string description{get;set;}
            // public Data data{get;set;}
            // public string projectId{get;set;}
            Console.WriteLine("hshdhd");
            loggeract.type = log.type;
            loggeract.userid = log.id;
            loggeract.published=log.published;
            loggeract.description=log.description;
            loggeract.data=log.data;
            loggeract.projectId=log.projectId;
            loggerActivityService.Create(loggeract);
            Console.WriteLine (JsonConvert.SerializeObject (log));
            byte[] messagebuffer = Encoding.Default.GetBytes (JsonConvert.SerializeObject (log));
            _model.BasicPublish (BoardsExchangeName, "Kibin", null, messagebuffer);
        }
        // public static Tasks ConvertforGantt (Tasks message, KanbanUserStory userstory)
        // {
        //     message.Id=userstory.Id;
        //     message.UserStoryName=userstory.UserStoryName;
        //     return message;
        // }

    }
}