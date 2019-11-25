using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Kibin.Models;
using Kibin.RabbitMQ;
using Kibin.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kibin.Controllers {
    [Route ("api/[controller]")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ListController : ControllerBase {
        private readonly ListService _listService;
        private readonly UsersService _userService;
        private readonly ReportService _reportService;


        public ListController(ListService listService,UsersService UserService,ReportService reportService)
        {
            _listService = listService;
            _userService= UserService;
            _reportService=reportService;
            //actorId=HttpContext.User.Claims.Where(c=>c.Type=="id").FirstOrDefault().Value;
        }

        [HttpGet]
        public ActionResult<List<List>> Get () =>
            _listService.Get ();

        [HttpGet ("{id:length(24)}", Name = "GetList")]
        public ActionResult<List> Get (string id) {
            var list = _listService.Get (id);

            if (list == null) {
                return NotFound ();
            }

            return list;
        }

        [HttpGet ("listname/{name}")]
        public ActionResult<List> GetName (string name) {
            var list = _listService.GetName (name);

            if (list == null) {
                return NotFound ();
            }

            return list;
        }

        [HttpGet ("projectid/{projectid}")]
        public ActionResult<List<List>> GetProjectById (string projectid) {
            var list = _listService.GetProjectById (projectid);

            if (list == null) {
                return NotFound ();
            }

            int count = 0;
            foreach (var item in list) { count += 1; }
            // foreach(var item in list){ Console.WriteLine(item.index);}
            // var temp=list[0];
            // list[0]=list[2];
            // list[2]=temp;
            // 
            for (var i = 0; i < count; i += 1) {
                for (var j = i + 1; j < count; j += 1) {
                    if (list[i].index > list[j].index) {
                        var temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }

            for (var i = 0; i < count; i += 1) {
                list[i].index = i;
            }
            //foreach(var item in list){ Console.WriteLine(item.index);}

            return list;
        }

        [HttpPost]
        public IActionResult Create (List list) {

            var findallproject = _listService.GetProjectById(list.projectId);

            if (findallproject == null)
            {
                list.index = 0;
                

            } else {
                int highestindex = 0;
                foreach (var i in findallproject) {
                    if (i.index >= highestindex) highestindex = i.index;
                }
                list.index = highestindex + 1;


            }
            _listService.Create(list);  
            var findallreport= _reportService.GetByProjectId(list.projectId);
            Console.WriteLine("projects{0}",JsonConvert.SerializeObject(findallreport));
            if(findallreport==null) 
            {
                Console.WriteLine("list");
                Report report= new Report();
                report.project_id=list.projectId;
                UserStories us = new UserStories();
                List<UserStories> _list= new List<UserStories>();
                var listData= _listService.GetProjectById(list.projectId);
                foreach(var x in listData)
                {
                    if(x.name==list.name)
                    {
                        us.id=x.Id;
                        us.label=x.name;
                        us.date=DateTime.Today;
                        TimeSpan timeSpan= new TimeSpan(00,00,00,00);
                        us.date= us.date.Date+ timeSpan;
                        List<string> userIDs= new List<string>();
                        us.userstoryid= userIDs;
                        _list.Add(us);
                        report.list=_list;
                        Console.WriteLine("Repport:{0}",JsonConvert.SerializeObject(report));
                        _reportService.Create(report);
                        break;
                    }
                }

                
            }
            else{
               UserStories us =new UserStories();
               var listData= _listService.GetProjectById(list.projectId);
               foreach(var x in listData)
               {
                   if(x.name==list.name)
                   {
                       us.id=list.Id;
                       us.label=list.name;
                       us.date=DateTime.Today;
                       TimeSpan timeSpan= new TimeSpan(12,00,00);
                       us.date= us.date.Date+ timeSpan;
                       List<string> userIDs= new List<string>();
                       us.userstoryid= userIDs;
                       Console.WriteLine("Got Reports:{0}",JsonConvert.SerializeObject(findallreport));
                       findallreport.list.Add(us);
                       Console.WriteLine("Reports:{0}",JsonConvert.SerializeObject(findallreport));
                       _reportService.UpdateAll(findallreport.project_id,findallreport);
                       Console.WriteLine("Fetched Report:{0}",JsonConvert.SerializeObject(_reportService.GetByProjectId(list.projectId)));
                       
                 }
               }


            }
            
            
           
            Logger actlog = new Logger()
            {
                type = "Activity@CreateList",
                id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                description = _userService.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " created a list named "
                + list.name,
                published = DateTime.Now,
                data = new Data()
                {
                    id = list.Id,
                    name = list.name
                }
            };
            RabbitMQProducer actProducer = new RabbitMQProducer();
            actProducer.SendMessageToActivityLogger(actlog);

            Logger log = new Logger()
            {
                type = "KanBan@CreateList",
                id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                description = "List created",
                published = DateTime.Now,
                data = new Data()
                {
                    id = list.Id,
                    name = list.name
                }
            };
            RabbitMQProducer producer = new RabbitMQProducer();
            producer.SendMessageToLogger(log);
            Console.WriteLine(log);
            

            return CreatedAtRoute ("GetList", new { id = list.Id.ToString () }, list);
        }

        [HttpPost ("sendmail")]
        public void SendEmail (string usid, string boardid, string role) {
            // Console.WriteLine(usid);
            // Console.WriteLine(boardid);

            Console.WriteLine ("email called list controller");
            MailMessage mailMessage = new MailMessage ("kibinmailer@gmail.com", _userService.Get (usid).email); //to , from
            //MailMessage mailMessage = new MailMessage("kibinmailer@gmail.com","paulshubhajyoti@gmail.com"); //to , from
            Console.WriteLine (_userService.Get (usid).email);
            mailMessage.Subject = "You are invited to join a board";
            mailMessage.Body = "Hi " + _userService.Get (usid).username +
                ",\nYou have been invited to join a board by " +
                _userService.Get (HttpContext.User.Claims.Where (c => c.Type == "id").FirstOrDefault ().Value).username +
                ".Please click the following link- \n http://localhost:4200/invitation/" + usid + "/" + boardid + "/" + role;

            SmtpClient smtpClient = new SmtpClient ("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential () {
                UserName = "kibinmailer@gmail.com",
                Password = "kibinmailer96"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send (mailMessage);
        }

        [HttpPut ("{id:length(24)}")]
        public IActionResult Update (string id, List listIn) {
            var list = _listService.Get (id);


            if (list == null) {
                return NotFound ();
            }

            // list name change
            // index of list changed
            // card added to list delete 

            _listService.Update(id, listIn);
              
            if(list.UserStory.Length<listIn.UserStory.Length)
            {
                var report= _reportService.GetByProjectId(listIn.projectId);
                foreach(var x in report.list)
                {
                    if(x.label==listIn.name)
                    {
                        Console.WriteLine("Date in db :{0}",x.date.Date);
                        Console.WriteLine("Data Todaye:{0}",DateTime.Today);
                        if(DateTime.Compare(x.date.Date,DateTime.Today.Date)==0)
                        {
                            foreach(var u in listIn.UserStory)
                            {
                                if(x.userstoryid.IndexOf(u.UserStoryId)>=0)
                                {
                                    continue;
                                }
                                else{
                                    x.userstoryid.Add(u.UserStoryId);
                                }
                            }
                            Console.WriteLine("New Story Added on Same Date:{0}",JsonConvert.SerializeObject(report));
                            _reportService.UpdateAll(report.project_id,report);
                            Console.WriteLine("Checking for Updated:{0}",JsonConvert.SerializeObject(_reportService.GetByProjectId(report.project_id)));
                        }
                        else{
                            UserStories us = new UserStories();
                            us.id= id;
                            us.label=listIn.name;
                            us.date=DateTime.Today;
                            List<string> userIds= new List<string>();
                            foreach(var li in x.userstoryid)
                            {
                                us.userstoryid.Add(li);
                            }
                            foreach(var li in listIn.UserStory)
                            {
                                Console.WriteLine("Testing:{0}",li.UserStoryId);
                                
                                if(us.userstoryid.Contains(li.UserStoryId))
                                {   Console.WriteLine("Found");
                                    continue;
                                }
                                else{
                                     Console.WriteLine("xxxxxxxxxxxxxxxxxxxNot Foundxxxxxxxxxxxxxxxx");
                                    us.userstoryid.Add(li.UserStoryId);
                                }
                            }
                            report.list.Add(us);
                            
                            _reportService.UpdateAll(report.project_id,report);                            
                        }
                        break;
                    }
                }
            }
            if(list.name!=listIn.name)
            {
                var report=_reportService.GetByProjectId(list.projectId);
                foreach(var x in report.list)
                {
                    if(list.name==x.label)
                    {
                        x.label=listIn.name;
                    }
                }
                Console.WriteLine("Name Set:{0}",JsonConvert.SerializeObject(report));
                _reportService.UpdateAll(report.project_id,report);
                Console.WriteLine("Fetched Name:{0]",JsonConvert.SerializeObject(_reportService.GetByProjectId(report.project_id)));
            }
            
                      
               
               
               



            if (list.name != listIn.name)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateList",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _userService.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " changed list name from "
                + list.name+" to "+listIn.name,
                    published = DateTime.Now,
                    projectId=list.projectId,
                    data = new Data()
                    {
                        id = listIn.Id,
                        name = listIn.name
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }
            else if (list.index != listIn.index)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateList",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _userService.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " changed position the list named "
                + listIn.name,
                    published = DateTime.Now,
                    projectId=list.projectId,
                    data = new Data()
                    {
                        id = listIn.Id,
                        name = listIn.name
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }
            else if (list.UserStory.Length > listIn.UserStory.Length)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateList",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _userService.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " removed a card from "
                +listIn.name,
                    published = DateTime.Now,
                    projectId=list.projectId,
                    data = new Data()
                    {
                        id = listIn.Id,
                        name = listIn.name
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }
            else if (list.UserStory.Length < listIn.UserStory.Length)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateList",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _userService.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " added a card to "
                +listIn.name,
                    published = DateTime.Now,
                    projectId=list.projectId,
                    data = new Data()
                    {
                        id = listIn.Id,
                        name = listIn.name
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }
            else if (list.UserStory.Length == listIn.UserStory.Length)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateList",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _userService.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " rearranged cards in "
                +listIn.name,
                    published = DateTime.Now,
                    projectId=list.projectId,
                    data = new Data()
                    {
                        id = listIn.Id,
                        name = listIn.name
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }


            Logger log = new Logger()
            {
                type = "KanBan@UpdateList",
                id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                description = "List updated",
                published = DateTime.Now,
                projectId=list.projectId,
                data = new Data()
                {
                    id = listIn.Id,
                    name = listIn.name
                }
            };
            RabbitMQProducer producer = new RabbitMQProducer();
            producer.SendMessageToLogger(log);
            Console.WriteLine(log);

            return NoContent ();
        }

        [HttpDelete ("{id:length(24)}")]
        public IActionResult Delete (string id) {
            var list = _listService.Get (id);

            if (list == null) {
                return NotFound ();
            }

            Logger actlog = new Logger () {
                type = "Activity@DeleteList",
                id = HttpContext.User.Claims.Where (c => c.Type == "id").FirstOrDefault ().Value,
                description = _userService.Get (HttpContext.User.Claims.Where (c => c.Type == "id").FirstOrDefault ().Value).username + " deleted a list named " +
                list.name,
                published = DateTime.Now,
                projectId = list.projectId,
                data = new Data () {
                id = list.Id,
                name = list.name
                }
            };
            RabbitMQProducer actProducer = new RabbitMQProducer ();
            actProducer.SendMessageToActivityLogger (actlog);
            Logger log = new Logger () {
                type = "KanBan@DeleteList",
                id = HttpContext.User.Claims.Where (c => c.Type == "id").FirstOrDefault ().Value,
                description = "List deleted",
                projectId = list.projectId,
                published = DateTime.Now,
                data = new Data () {
                id = id,
                name = _listService.Get (id).name
                }
            };
            RabbitMQProducer producer = new RabbitMQProducer ();
            producer.SendMessageToLogger (log);
            Console.WriteLine (log);
            _listService.Remove (list.Id);

            return NoContent ();
        }
    }
}