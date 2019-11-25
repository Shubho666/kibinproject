using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Kibin.Services;
using Newtonsoft.Json;
using Kibin.RabbitMQ;
using Kibin.Models;





namespace Kibin.RabbitMQ
{
     public class RabbitMQReceiver
    {   
        
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
       private const string IdeaZoneQueueName = "IdeaToGantt";
       
        private const string BoardsQueueName = "BoardsToGantt";
        // static KibinDatabaseSettings  settings = new KibinDatabaseSettings(){
        //     ConnectionString = "mongodb://localhost:27017",
        //     DatabaseName= "KibinDbKanban"
        // };
        static MongoDBContext ctx = new MongoDBContext();
       // var UsersService = new UsersService (ctx);
        static TaskService taskService = new TaskService(ctx);
        public RabbitMQReceiver()
        {   
            // KanbanUserService= new KanbanUSService(settings);
             _factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RabbitMQURL"),
                // Port = 5672,
                // VirtualHost = "/",
                UserName = "guest",
                Password = "guest"
            };
            _connection = _factory.CreateConnection();

            ReceivefromIdeaZone();
            ReceivefromBoards();
        }
        public static void ReceivefromIdeaZone()
        {
    

            _channel = _connection.CreateModel();
            {
                var queueName = _channel.QueueDeclare(
                    queue: IdeaZoneQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
              

                // _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);


                Console.WriteLine(" [*] Waiting for Ideas.");

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    string message = Encoding.Default.GetString(body);
                    Console.WriteLine(" [x] ----> {0}", message);
                    var  userstory= JsonConvert.DeserializeObject<UserStoryIdeas>(message);
                    


                     Console.WriteLine("Object:{0}",userstory.linkedtoId);
                     //Tasks sendData = new Tasks();
                    //  Tasks1 send=new Tasks1();
                     //sendData = GetUserStoryfromIdeas(sendData,userstory);
                     //taskService.Create(sendData);
                     //Console.WriteLine("text:{0}",sendData.text);
                    
                    DateTime k1=DateTime.Now;
                    long sd=((DateTimeOffset)k1).ToUnixTimeMilliseconds();

                    DateTime k2=k1.AddDays(2);
                    long ed=((DateTimeOffset)k2).ToUnixTimeMilliseconds();

                    if (userstory.action=="post"){
                        Tasks sendData = new Tasks();
                       
                        long a=RabbitMQReceiver.LongRandom(1000000000000, 1000001000000, new Random());
                         sendData.id=a;
                         sendData.project_id=userstory.projectId;
                         sendData.start_date=sd;
                         sendData.end_date=ed;
                         sendData.text=userstory.description;
                         sendData.duration=2;
                        sendData.unique_id=userstory.linkedtoId;
                        //sendData.duration=2;
                        
                        taskService.Create(sendData);
                     }
                    //  else if (userstory.action=="put"){
                    //      Tasks1 send=new Tasks1();
                    //      send.project_id=userstory.projectId;
                    //      send.start_date=sd;
                    //      send.end_date=ed;
                    //      send.text=userstory.description;
                    //      send.project_id=userstory.projectId;
                    //     send.unique_id=userstory.linkedtoId;
    
                    //     taskService.PutByUniqueId(send.unique_id,send);
                    //  }
                     else if(userstory.action=="delete"){

                         string a =userstory.linkedtoId;

                         taskService.RemoveByUniqueId(a);

                     }

                 
                };
                _channel.BasicConsume(queue: IdeaZoneQueueName,
                                     autoAck: true,
                                     consumer: consumer
                                     );


                Console.WriteLine(" Press [enter] to exit.");
                // Console.ReadLine();
            }
        }

         public static void ReceivefromBoards()
        {
    

            _channel = _connection.CreateModel();
            {
                var queueName = _channel.QueueDeclare(
                    queue: BoardsQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                 

                // _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);


                Console.WriteLine(" [*] Waiting for Boards.");

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    string message = Encoding.Default.GetString(body);
                    Console.WriteLine(" [x] ----> {0}", message);
                    var  userstory= JsonConvert.DeserializeObject<KanbanUserStory>(message);
                    


                     Console.WriteLine("Object:{0}",userstory.Id);
                     
                    DateTime k1=userstory.startTime;
                    long sd=((DateTimeOffset)k1).ToUnixTimeMilliseconds();

                    DateTime k2=k1.AddDays(2);
                    long ed=((DateTimeOffset)k1).ToUnixTimeMilliseconds();

                    //Service.Create(sendData);
                    //  }
                      if (userstory.action=="put"){
                        Tasks send1=new Tasks();
                       send1=taskService.GetByUniqueId(userstory.uniqueId);
                       if(send1==null)
                        { send1=taskService.Get(userstory.uniqueId);}

                          DateTime k3=userstory.startTime;
                          long start=((DateTimeOffset)k3).ToUnixTimeMilliseconds();

                          DateTime k4=userstory.endTime;
                          long end=((DateTimeOffset)k4).ToUnixTimeMilliseconds();
                        
                        ///send1.TaskId=send1.TaskId;
                        
                         send1.progress=userstory.progress;
                        send1.text=userstory.shortName;
                         send1.start_date=start;
                         send1.end_date=end;
                         decimal d=(send1.end_date-send1.start_date)/86400000;
                        send1.duration=(int)Math.Round(d);
                        // Console.WriteLine("Hello1");
                        // Console.WriteLine(send1);
                        taskService.Update(send1.TaskId,send1);
                     }
                 
                };
                _channel.BasicConsume(queue: BoardsQueueName,
                                     autoAck: true,
                                     consumer: consumer
                                     );

                
                Console.WriteLine(" Press [enter] to exit.");
                // Console.ReadLine();
            }
        }

        
        public void Close()
        {
            _connection.Close();
        }

      
    public static long LongRandom(long min, long max, Random rand) {
       byte[] buf = new byte[8];
       rand.NextBytes(buf);
       long longRand = BitConverter.ToInt64(buf, 0);

       return (Math.Abs(longRand % (max - min)) + min);}
        
    }
}

