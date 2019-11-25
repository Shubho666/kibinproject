using System;
using System.Collections.Generic;
using Kibin.Models;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Kibin.RabbitMQ;
using System.Text;
using Microsoft.Extensions.Logging;
using Kibin.Services;

namespace Kibin.RabbitMQ
{
    public class RabbitMQProducer
    {
        static MongoDBContext ctx = new MongoDBContext();
        private static LoggerActivityService _loggerService = new LoggerActivityService(ctx);

        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private const string IdeaExchange = "IdeaZone_Exchange";
        private const string LoggerQueue = "Idea";
        private const string GanttQueue = "IdeaToGantt";
        private const string BoardQueue = "IdeaToBoard";
        public RabbitMQProducer()
        {
            CreateConnection();
        }
        private static void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName =  Environment.GetEnvironmentVariable("RabbitMQURL"),
                UserName = "guest",
                Password = "guest"
            };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(IdeaExchange, "topic" );
            _model.QueueDeclare(LoggerQueue, true, false, false, null);
            _model.QueueDeclare(GanttQueue, true, false, false, null);
            _model.QueueDeclare(BoardQueue, true, false, false, null);
            _model.QueueBind( LoggerQueue, IdeaExchange, "Kibin");
            _model.QueueBind(GanttQueue, IdeaExchange, "GanttRoute");
            _model.QueueBind(BoardQueue, IdeaExchange, "BoardRoute");
        }
        public void Close()
        {
            _connection.Close();
        }
        public void SendBacklog(UserStoryIdeas userstory)
        {
            byte[] message = Encoding.Default.GetBytes(JsonConvert.SerializeObject(userstory));
            _model.BasicPublish(IdeaExchange, "BoardRoute", null, message);
            _model.BasicPublish(IdeaExchange, "GanttRoute", null, message);
            Console.WriteLine(JsonConvert.SerializeObject(message));
        }
        //public void SendBacklogBoard(UserStoryIdeas userstory)
        //{
        //    byte[] messageBoard = Encoding.Default.GetBytes(JsonConvert.SerializeObject(userstory));
        //    _model.BasicPublish(IdeaExchange, "BoardRoute", null, messageBoard);
        //    Console.WriteLine(JsonConvert.SerializeObject(messageBoard));
        //}
        public void SendMessageToLoggerD(Logger_Domain log)
        {
            Console.WriteLine(log);
            Console.WriteLine("producer log");
            Console.WriteLine(JsonConvert.SerializeObject(log));
            byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
            _model.BasicPublish(IdeaExchange, "Kibin", null, messagebuffer);
        }
        public void SendMessageToLoggerA(Logger_Activity log)
        {
            LoggerActivity loggeract = new LoggerActivity();
            //     public string type{get;set;}

            // public string userid{get;set;}    //    UserId
            // public DateTime published{get;set;}
            // public string description{get;set;}
            // public Data data{get;set;}
            // public string projectId{get;set;}
            loggeract.type = log.type;
            loggeract.userid = log.id;
            loggeract.published = log.published;
            loggeract.description = log.description;
            loggeract.data = log.details;
            loggeract.projectId = log.projectId;
            _loggerService.Create(loggeract);


            Console.WriteLine(log);
            Console.WriteLine("producer log");
            Console.WriteLine(JsonConvert.SerializeObject(log));
            byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
            _model.BasicPublish(IdeaExchange, "Kibin", null, messagebuffer);
        }
    }
}