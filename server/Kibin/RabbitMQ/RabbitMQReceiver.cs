using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.SignalR;
using Kibin.Services;
//using Kibin.SignalR_Hub;
using Newtonsoft.Json;
using Kibin.RabbitMQ;
using Kibin.Models;





namespace Kibin.RabbitMQ
{
    class RabbitMQReceiver
    {

        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        private const string IdeaZoneQueueName = "IdeaToBoard";

        private const string GanttQueueName = "GanttToBoards";
        // static KibinDatabaseSettings  settings = new KibinDatabaseSettings(){
        //     ConnectionString = "mongodb://localhost:27017",
        //     DatabaseName= "KibinDbKanban"
        // };
        //RabbitMQReceiver b=new RabbitMQReceiver();
        static MongoDBContext ctx = new MongoDBContext();
        // var UsersService = new UsersService (ctx);
        static KanbanUSService userService = new KanbanUSService(ctx);
        static ListService listService = new ListService(ctx);

        public RabbitMQReceiver()
        {
            // KanbanUserService= new KanbanUSService(settings);
            _factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                //Port = 5672,
                // VirtualHost = "/",
                UserName = "guest",
                Password = "guest"
            };
            _connection = _factory.CreateConnection();

            ReceivefromIdeaZone();

            ReceivefromGantt();
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

                     
                     //Console.WriteLine("coming here");
                    var body = ea.Body;
                    string message = Encoding.Default.GetString(body);
                    Console.WriteLine(" [x] ----> {0}", message);
                    var userstoryfromidea = JsonConvert.DeserializeObject<UserStoryIdeas>(message);

                   

                    Console.WriteLine("Object:{0}", userstoryfromidea.linkedtoId);
                    //KanbanUserStory sendData = new KanbanUserStory();
                    //  sendData = GetUserStoryfromGantt(sendData,userstory);
                    //  userService.Create(sendData);
                    //  Console.WriteLine("text:{0}",sendData);
                    
                    //b.SendUserStoryToAddOnList(userstoryfromidea.projectId,"success");
                    //Clients.Group(userstoryfromidea.projectId).SendAsync("RabbitListUpdate","success");

                    if (userstoryfromidea.action == "post")
                    {
                        KanbanUserStory sendData = new KanbanUserStory();
                        sendData.uniqueId = userstoryfromidea.linkedtoId;
                        sendData.shortName = userstoryfromidea.description;
                        //sendData.acceptanceCriteria = userstoryfromidea.acceptanceCriteria;

                        if(userstoryfromidea.acceptanceCriteria==null)
                        {
                            sendData.acceptanceCriteria=new string[0]{};
                        }
                        else
                        {
                            sendData.acceptanceCriteria=userstoryfromidea.acceptanceCriteria;
                        }
                        sendData.linkedToId=userstoryfromidea.linkedtoId;
                        sendData.startTime=DateTime.Now;
                        sendData.endTime=DateTime.Now;
                        sendData.status = "Product Backlog";
                        //sendData.linkedToId = userstoryfromidea.linkedtoId;
                        sendData.projectId = userstoryfromidea.projectId;
                        sendData.tasks=new USTask[0]{};
                        sendData.assignedTo=new AssignedTo[0]{};
                        sendData.startTime=DateTime.Now;
                        sendData.endTime=DateTime.Now;

                        //sendData.progress = userstoryfromidea.progress;
                        var userstoryCreated = userService.Create(sendData);

                        //var kanbanUS = userService.GetByProjectId(userstoryfromidea.projectId);
                        var listobj = listService.GetByProjectIdAndPB(sendData.projectId);

                        if (listobj == null)
                        {
                            Console.WriteLine("Inside list obj == null");
                            List objr = new List()
                            {

                                name = "Product Backlog",
                                projectId = sendData.projectId,
                                index = 0,
                                UserStory = new UserStory[1]{new UserStory(){UserStoryId=userstoryCreated.Id,
                                UserStoryName=sendData.shortName}

                                }
                            };
                            listService.Create(objr);

                        }
                        else
                        {
                            Console.WriteLine("obj not null so updating list");
                            //List<UserStory> obj=listobj.UserStory.ToList();
                            UserStory abc = new UserStory()
                            {
                                UserStoryId = userstoryCreated.Id,
                                UserStoryName = sendData.shortName
                            };


                            //listobj.UserStory=listobj.UserStory.Concat(abc).ToArray();

                            // Array.Resize(ref listobj.UserStory, listobj.UserStory.Length + 1);
                            // listobj.UserStory[listobj.UserStory.Length - 1] = abc;
                            List<UserStory> uslist = new List<UserStory>();
                            for (int i = 0; i < listobj.UserStory.Length; i++)
                            {
                                uslist.Add(listobj.UserStory[i]);
                            }
                            uslist.Add(abc);
                            listobj.UserStory = uslist.ToArray();

                            // listobj.UserStory=listobj.UserStory.ToList().Add(new UserStory(){UserStoryId=userstoryCreated.Id,
                            // UserStoryName=sendData.shortName}).ToArray();///
                            listService.Update(listobj.Id, listobj);

                            

                        }
                        //userService.SendRabbit(userstoryfromidea.projectId);
                        //var kmm=RabbitMQReceiver.b.SendUserStoryToAddOnList(userstoryfromidea.projectId,"aadasdasd");
                    }
                    else if(userstoryfromidea.action=="delete")
                    {
                        Console.WriteLine("inside delete now");
                        var getuserstory=userService.GetByUniqueId(userstoryfromidea.linkedtoId);
                        userService.RemoveByUniqueId(userstoryfromidea.linkedtoId);
                        var allLists=listService.GetProjectById(userstoryfromidea.projectId);
                        var linkedToCards=userService.GetByLinkedId(userstoryfromidea.linkedtoId);
                        // Console.WriteLine(linkedToCards+"linked");
                        foreach(var i in linkedToCards)
                        {
                            userService.Remove(i.Id);
                            Console.WriteLine(i.shortName);
                            foreach(var j in allLists){
                                List<UserStory> linklist=new List<UserStory>();
                                foreach(var k in j.UserStory){
                                    if(k.UserStoryId!=i.Id){
                                        linklist.Add(k);
                                    }
                                }
                               j.UserStory=linklist.ToArray();
                               listService.Update(j.Id,j); 
                            }
                        }
                        string listidwherefound=allLists[0].Id;
                            foreach(var i in allLists)
                            {
                                foreach( var j in i.UserStory)
                                {
                                    if(j.UserStoryId==getuserstory.Id){
                                        listidwherefound=i.Id;
                                        Console.WriteLine("list id {0}",listidwherefound);
                                    }
                                }

                            }

                            var listobj=listService.Get(listidwherefound);
                            //var listobj=listwhereidfound.UserStory;
                            List<UserStory> uslist = new List<UserStory>();
                            for (int i = 0; i < listobj.UserStory.Length; i++)
                            {
                                if(listobj.UserStory[i].UserStoryId==getuserstory.Id){}
                                else{uslist.Add(listobj.UserStory[i]);}
                            }
                            //uslist.Add(abc);
                            listobj.UserStory = uslist.ToArray();
                            listService.Update(listobj.Id, listobj);



                    }


                    //else if (taskfromgantt.action == "delete")
                    //{
                    //    var kanbanUS = userService.GetByUniqueId(taskfromgantt.unique_id);
                    //    if (kanbanUS == null)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        userService.Remove(kanbanUS.Id);
                    //    }

                    //}



                };
                _channel.BasicConsume(queue: IdeaZoneQueueName,
                                     autoAck: true,
                                     consumer: consumer
                                     );


                Console.WriteLine(" Press [enter] to exit.");
                // Console.ReadLine();
            }
        }

        // public System.Threading.Tasks.Task SendUserStoryToAddOnList (string boardid,string msg)
        // {
        //     return Clients.Group(boardid).SendAsync("RabbitListUpdate",msg);
        // }

        public static void ReceivefromGantt()
        {


            _channel = _connection.CreateModel();
            {
                var queueName = _channel.QueueDeclare(
                    queue: GanttQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);


                // _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);


                Console.WriteLine(" [*] Waiting for Gantt.");

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    string message = Encoding.Default.GetString(body);
                    Console.WriteLine(" [x] ----> {0}", message);
                    var taskfromgantt = JsonConvert.DeserializeObject<Tasks>(message);



                    Console.WriteLine("Object:{0}", taskfromgantt.TaskId);
                    //KanbanUserStory sendData = new KanbanUserStory();
                    //  sendData = GetUserStoryfromGantt(sendData,userstory);
                    //  userService.Create(sendData);
                    //  Console.WriteLine("text:{0}",sendData);

                    if (taskfromgantt.action == "post")
                    {
                        KanbanUserStory sendData = new KanbanUserStory();
                        if(taskfromgantt.unique_id==null)
                        {sendData.uniqueId = taskfromgantt.TaskId;}
                        else{
                            sendData.uniqueId=taskfromgantt.unique_id;
                        }
                        sendData.linkedToId=taskfromgantt.TaskId;
                        sendData.shortName = taskfromgantt.text;
                        
                        sendData.tasks=new USTask[0]{};
                        sendData.acceptanceCriteria=new string[0]{};
                        sendData.assignedTo=new AssignedTo[0]{};
                        sendData.startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(taskfromgantt.start_date).ToLocalTime();
                        sendData.endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(taskfromgantt.end_date).ToLocalTime();
                        sendData.projectId = taskfromgantt.project_id;
                        Console.WriteLine(sendData.startTime);
                        var userstoryCreated=userService.Create(sendData);

                        var listobj=listService.GetByProjectIdAndPB(sendData.projectId);
                        if(listobj==null)
                        {
                            List objr=new List(){
                                name = "Product Backlog",
                                projectId = sendData.projectId,
                                index = -1,
                                UserStory = new UserStory[1]{new UserStory(){UserStoryId=userstoryCreated.Id,
                                UserStoryName=sendData.shortName}}
                            };
                            listService.Create(objr);
                        }
                        else{
                            UserStory abc = new UserStory()
                            {
                                UserStoryId = userstoryCreated.Id,
                                UserStoryName = sendData.shortName
                            };


                            List<UserStory> uslist = new List<UserStory>();
                            for (int i = 0; i < listobj.UserStory.Length; i++)
                            {
                                uslist.Add(listobj.UserStory[i]);
                            }
                            uslist.Add(abc);
                            listobj.UserStory = uslist.ToArray();
                            listService.Update(listobj.Id, listobj);

                        }



                    }
                    else if (taskfromgantt.action == "put")
                    {
                        
                        
                        var kanbanUS = userService.GetByUniqueId(taskfromgantt.TaskId);
                        if(taskfromgantt.unique_id==null){}
                        else{
                            kanbanUS=userService.GetByUniqueId(taskfromgantt.unique_id);
                        }

                        if (kanbanUS == null)
                        {

                        }
                        else
                        {
                            //userService.Update(kanbanUS.Id,KanbanUserStory userIn);
                            kanbanUS.progress = taskfromgantt.progress;
                            kanbanUS.shortName = taskfromgantt.text;
                            kanbanUS.startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(taskfromgantt.start_date).ToLocalTime();
                            kanbanUS.endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(taskfromgantt.end_date).ToLocalTime();
                            userService.Update(kanbanUS.Id, kanbanUS);

                            var allLists=listService.GetProjectById(kanbanUS.projectId);
                            foreach(var i in allLists)
                            {
                                foreach( var j in i.UserStory)
                                {
                                    if(j.UserStoryId==kanbanUS.Id){
                                        j.UserStoryName=kanbanUS.shortName;
                                        listService.Update(i.Id,i);
                                    }
                                }

                            }


                        }

                    }
                    else if (taskfromgantt.action == "delete")
                    {
                        var kanbanUS = userService.GetByUniqueId(taskfromgantt.TaskId);
                        var linkToCards=userService.GetByLinkedId(taskfromgantt.TaskId);
                        if(taskfromgantt.unique_id==null){}
                        else{
                            kanbanUS=userService.GetByUniqueId(taskfromgantt.unique_id);
                            linkToCards=userService.GetByLinkedId(taskfromgantt.unique_id);

                        }
                        Console.WriteLine(kanbanUS.Id);
                        if (kanbanUS == null)
                        {

                        }
                        else
                        {
                            userService.Remove(kanbanUS.Id);
                            var allLists=listService.GetProjectById(kanbanUS.projectId);
                            foreach(var i in linkToCards){
                                userService.Remove(i.Id);
                                foreach(var j in allLists){
                                    List<UserStory> linklist=new List<UserStory>();
                                    foreach(var k in j.UserStory){
                                        if(k.UserStoryId!=i.Id){
                                            linklist.Add(k);
                                        }
                                    }
                                    j.UserStory=linklist.ToArray();
                                    listService.Update(j.Id,j);
                                }
                            }

                            string listidwherefound=allLists[0].Id;
                            foreach(var i in allLists)
                            {
                                foreach( var j in i.UserStory)
                                {
                                    if(j.UserStoryId==kanbanUS.Id){
                                        listidwherefound=i.Id;
                                    }
                                }

                            }

                            var listobj=listService.Get(listidwherefound);
                            //var listobj=listwhereidfound.UserStory;
                            List<UserStory> uslist = new List<UserStory>();
                            for (int i = 0; i < listobj.UserStory.Length; i++)
                            {
                                if(listobj.UserStory[i].UserStoryId==kanbanUS.Id){}
                                else{uslist.Add(listobj.UserStory[i]);}
                            }
                            //uslist.Add(abc);
                            listobj.UserStory = uslist.ToArray();
                            listService.Update(listobj.Id, listobj);


                        }

                    }



                };
                _channel.BasicConsume(queue: GanttQueueName,
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
        

        //public static KanbanUserStory GetUserStoryfromIdeas(KanbanUserStory sendData,UserStoryIdeas userstory)
        //{ 
        //sendData.Id= userstory.linkedtoId;
        ////sendData.shortName= userstory.UserStoryName;


        //sendData.progress=0;

        //return sendData;
        //}

        //public static KanbanUserStory GetUserStoryfromGantt(KanbanUserStory sendData,Tasks userstory)
        //{ 
        //sendData.Id= userstory.TaskId;
        //sendData.shortName= userstory.text;

        //sendData.progress=userstory.progress;

        //return sendData;
        //}
    }
}