// using System;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System.Text;
// using Kibin.Services;
// using Newtonsoft.Json;
// using Kibin.Models;




// namespace Kibin.RabbitMQ
// {
//     class RabbitMQReceiver
//     {   
        
//         private static ConnectionFactory _factory;
//         private static IConnection _connection;
//         private static IModel _channel;
//         private const string AllQueueName = "q1";
//         static KibinDatabaseSettings  settings = new KibinDatabaseSettings(){
//             ConnectionString = "mongodb://localhost:27017",
//             DatabaseName= "KibinDbKanban"
//         };
//         static KanbanUSService KanbanUserService = new KanbanUSService(settings);
//         public RabbitMQReceiveLogs()
//         {   
//             // KanbanUserService= new KanbanUSService(settings);
//             Receiver();
//         }
//         public static void Receiver()
//         {
//             _factory = new ConnectionFactory()
//             {
//                 HostName = "localhost",
//                 // Port = 5672,
//                 // VirtualHost = "/",
//                 UserName = "guest",
//                 Password = "guest"
//             };
//             _connection = _factory.CreateConnection();

//             _channel = _connection.CreateModel();
//             {
//                 var queueName = _channel.QueueDeclare(
//                     queue: AllQueueName,
//                     durable: true,
//                     exclusive: false,
//                     autoDelete: false,
//                     arguments: null);

//                 // _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);


//                 Console.WriteLine(" [*] Waiting for logs.");

//                 var consumer = new EventingBasicConsumer(_channel);
//                 consumer.Received += (model, ea) =>
//                 {
//                     var body = ea.Body;
//                     string message = Encoding.Default.GetString(body);
//                     Console.WriteLine(" [x] ----> {0}", message);
//                     var  userstory= JsonConvert.DeserializeObject<KanbanUserStoryIdea>(message);
                    


//                      Console.WriteLine("Object:{0}",userstory.Id);
//                      KanbanUserStory sendData = new KanbanUserStory();
//                      sendData = GetUserStory(sendData,userstory);
//                      KanbanUserService.Create(sendData);


                 
//                 };
//                 _channel.BasicConsume(queue: AllQueueName,
//                                      autoAck: true,
//                                      consumer: consumer
//                                      );


//                 Console.WriteLine(" Press [enter] to exit.");
//                 // Console.ReadLine();
//             }
//         }
//         public void Close()
//         {
//             _connection.Close();
//         }
//         public static KanbanUserStory GetUserStory(KanbanUserStory sendData,KanbanUserStoryIdea userstory)
//         { sendData.Id= userstory.Id;
//         sendData.description= userstory.description;
//         sendData.shortName=userstory.shortName;
//         sendData.projectId=userstory.projectId;
//         sendData.userId=userstory.userId;
//         sendData.acceptanceCriteria=userstory.acceptanceCriteria;
//         sendData.status=userstory.status;
        



//             return sendData;
//         }
//     }
// }