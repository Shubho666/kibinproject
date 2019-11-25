using System;
using System.Collections.Generic;
using Kibin.Models;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Kibin.RabbitMQ;
using System.Text;
namespace Kibin.RabbitMQ
{
    public class RabbitMQProducer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string ExchangeName = "Gantt_Exchange";
        private const string LoggerQueueName = "Gantt";
        private const string BoardsQueueName = "GanttToBoards";

  

        public RabbitMQProducer()
        {
            CreateConnection();
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RabbitMQURL"), UserName = "guest", Password = "guest"
               //HostName = "172.23.239.55",Port = 5672, UserName = "guest", Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "topic",durable:true);

         
            _model.QueueDeclare(BoardsQueueName, true, false, false, null);
           

            _model.QueueBind(BoardsQueueName, ExchangeName, "BoardsKey");

            _model.QueueDeclare(LoggerQueueName, true, false, false, null);

            _model.QueueBind(LoggerQueueName, ExchangeName, "Kibin");
     
        }

        public void Close()
        {
            _connection.Close();
        }

        public void SendUserStoryToBoards(Tasks task)
        {
            Console.WriteLine(JsonConvert.SerializeObject(task));
            byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(task));
            _model.BasicPublish(ExchangeName,"BoardsKey", null, messagebuffer);  
        }

      
        public void UpdateUserStoryToBoards(Tasks task)
        {
            Console.WriteLine(JsonConvert.SerializeObject(task));
            byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(task));
             _model.BasicPublish(ExchangeName,"BoardsKey", null, messagebuffer);
        }
       
         public void DeleteUSerStoryFromBoards(Tasks task)
        {
            Console.WriteLine(JsonConvert.SerializeObject(task));
            byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(task));
            _model.BasicPublish(ExchangeName,"BoardsKey", null, messagebuffer);
        }

      

        // public void SendMessage(byte[] message, string routingKey)
        // {
        //     _model.BasicPublish(ExchangeName, routingKey, null, message);
        // }  

        //DOMAIN EVENTS      
        // public void AddUserStory(AddUS log){
          
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        // public void DelUserStory(DelUS log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        // public void UpdateUserStory(UpdUS log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        // public void AddLink(AddLink log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        // public void DelLink(DelLink log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        //ACTIVITIES

        // public void AddUserStoryAct(AddUS log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        // public void DelUserStoryAct(DelUS log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        // public void UpdateUserStoryAct(UpdUS log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        // public void AddLinkAct(AddLink log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

        // public void DelLinkAct(DelLink log){
        //    byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(log));
        //    _model.BasicPublish(ExchangeName, "Kibin",null,messagebuffer);
        // }

    }  
}