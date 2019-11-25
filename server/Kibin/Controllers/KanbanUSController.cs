using Kibin.Models;
using Kibin.Services;
using Kibin.RabbitMQ;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class KanbanUSController : ControllerBase
    {
        private readonly IKanbanUSService _userService;
        private readonly UsersService _user1Service;

        public KanbanUSController(IKanbanUSService UsersService, UsersService User1Service)
        {
            _userService = UsersService;
            _user1Service = User1Service;
        }

        [HttpGet]
        public ActionResult<List<KanbanUserStory>> Get() =>
            Ok(_userService.Get());

        [HttpGet("{id:length(24)}", Name = "GetBook1")]
        public ActionResult<KanbanUserStory> Get(string id)
        {
            var kanbanUS = _userService.Get(id);

            if (kanbanUS == null)
            {
                return NotFound();
            }

            return kanbanUS;
        }

        [HttpGet("userid/{userid}")]
        public ActionResult<KanbanUserStory> GetUserId(string userid)
        {
            var kanbanUS = _userService.GetUserId(userid);

            if (kanbanUS == null)
            {
                return NotFound();
            }


            return kanbanUS;
        }

        [HttpGet("projectid/{userid}")]
        public ActionResult<List<KanbanUserStory>> GetByProjectId(string userid)
        {
            var book = _userService.GetByProjectId(userid);

            if (book == null)
            {
                return NotFound();
            }


            return book;
        }

        [HttpGet("linkedid/{userid}")]
        public ActionResult<List<KanbanUserStory>> GetByLinkedId(string userid)
        {
            var book = _userService.GetByLinkedId(userid);

            if (book == null)
            {
                return NotFound();
            }


            return book;
        }

        [HttpGet("uniqueid/{userid}")]
        public ActionResult<KanbanUserStory> GetByUniqueId(string userid)
        {
            var book = _userService.GetByUniqueId(userid);

            if (book == null)
            {
                return NotFound();
            }


            return book;
        }

        [HttpGet("lastuserid/{userid}")]
        public ActionResult<KanbanUserStory> GetLastUserId(string userid)
        {
            var kanbanUS = _userService.GetLastUserId(userid);
            if (kanbanUS == null)
            {
                return NotFound();
            }
            KanbanUserStory kanbanUSsolo = new KanbanUserStory();
            foreach (var i in kanbanUS)
            {
                kanbanUSsolo = i;
            }
            return kanbanUSsolo;
        }



        [HttpPost]
        public IActionResult Create(KanbanUserStory kanbanUS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.Create(kanbanUS);

            KanbanData kanbanobj = new KanbanData()
            {
                Id = kanbanUS.Id,
                action = "post",
                uniqueId = null,
                description = kanbanUS.description,
                shortName = kanbanUS.shortName,
                projectId = kanbanUS.projectId,
                userId = kanbanUS.userId,
                status = kanbanUS.status,
                startTime = kanbanUS.startTime,
                endTime = kanbanUS.endTime,
                linkedToId = kanbanUS.linkedToId,
                assignedTo = kanbanUS.assignedTo,
                acceptanceCriteria = kanbanUS.acceptanceCriteria
            };

            //RabbitMQProducer prod =new RabbitMQProducer();
            //prod.ProduceMessageToGantt(kanbanobj);



            Logger actlog = new Logger()
            {
                type = "Activity@CreateCard",
                id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " created a card named "
                + kanbanUS.shortName,
                published = DateTime.Now,
                projectId= kanbanUS.projectId,
                data = new Data()
                {
                    id = kanbanUS.Id,
                    name = kanbanUS.shortName
                }
            };
            RabbitMQProducer actProducer = new RabbitMQProducer();
            actProducer.SendMessageToActivityLogger(actlog);
            Logger log = new Logger()
            {
                type = "KanBan@CreateCard",
                id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                description = "Card created",
                published = DateTime.Now,
                projectId=kanbanUS.projectId,
                data = new Data()
                {
                    id = kanbanUS.Id,
                    name = kanbanUS.shortName
                }
            };
            RabbitMQProducer producer = new RabbitMQProducer();
            producer.SendMessageToLogger(log);
            Console.WriteLine(log);

            return CreatedAtRoute("GetBook1", new { id = kanbanUS.Id.ToString() }, kanbanUS);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, KanbanUserStory kanbanUSIn)
        {
            var kanbanUS = _userService.Get(id);

            if (kanbanUS == null)
            {
                return NotFound();
            }
            //user story name change
            // subtask add delete change
            // task add delete
            // acceptance critera add del
            // start end point changed

            _userService.Update(id, kanbanUSIn);

            if (kanbanUS.shortName != kanbanUSIn.shortName)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateCard",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " changed title from "
                + kanbanUS.shortName + " to " + kanbanUSIn.shortName,
                    published = DateTime.Now,
                    projectId= kanbanUSIn.projectId,
                    data = new Data()
                    {
                        id = kanbanUSIn.Id,
                        name = kanbanUSIn.shortName
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }
            else if (kanbanUS.startTime != kanbanUSIn.startTime)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateCard",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " changed start time from "
                + kanbanUS.startTime + " to " + kanbanUSIn.startTime,
                
                    projectId= kanbanUSIn.projectId,
                    published = DateTime.Now,
                    data = new Data()
                    {
                        id = kanbanUSIn.Id,
                        name = kanbanUSIn.shortName
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }
            else if (kanbanUS.endTime != kanbanUSIn.endTime)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateCard",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " changed end time from "
                + kanbanUS.endTime + " to " + kanbanUSIn.endTime,
                projectId= kanbanUSIn.projectId,
                    published = DateTime.Now,
                    data = new Data()
                    {
                        id = kanbanUSIn.Id,
                        name = kanbanUSIn.shortName
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }
            else if (kanbanUSIn.points != kanbanUSIn.points)
            {
                Logger actlog = new Logger()
                {
                    type = "Activity@UpdateCard",
                    id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                    description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " changed points from "
                + kanbanUS.points + " to " + kanbanUSIn.points,
                    published = DateTime.Now,
                    projectId= kanbanUSIn.projectId,
                    data = new Data()
                    {
                        id = kanbanUSIn.Id,
                        name = kanbanUSIn.shortName
                    }
                };
                RabbitMQProducer actProducer = new RabbitMQProducer();
                actProducer.SendMessageToActivityLogger(actlog);

            }
            else if (kanbanUS.acceptanceCriteria != null)
            {
                if (kanbanUS.acceptanceCriteria.Length > kanbanUSIn.acceptanceCriteria.Length)
                {
                    Logger actlog = new Logger()
                    {
                        type = "Activity@UpdateCard",
                        id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                        description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " deleted an acceptance criteria",
                        published = DateTime.Now,
                        projectId= kanbanUSIn.projectId,
                        data = new Data()
                        {
                            id = kanbanUSIn.Id,
                            name = kanbanUSIn.shortName
                        }
                    };
                    RabbitMQProducer actProducer = new RabbitMQProducer();
                    actProducer.SendMessageToActivityLogger(actlog);

                }
                else if (kanbanUS.acceptanceCriteria.Length < kanbanUSIn.acceptanceCriteria.Length)
                {
                    Logger actlog = new Logger()
                    {
                        type = "Activity@UpdateCard",
                        id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                        description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " added an acceptance criteria",
                        published = DateTime.Now,
                        projectId= kanbanUSIn.projectId,
                        data = new Data()
                        {
                            id = kanbanUSIn.Id,
                            name = kanbanUSIn.shortName
                        }
                    };
                    RabbitMQProducer actProducer = new RabbitMQProducer();
                    actProducer.SendMessageToActivityLogger(actlog);

                }
            }

            else if (kanbanUS.tasks != null)
            {
                if(kanbanUS.tasks.Length > kanbanUSIn.tasks.Length)
            {
                    Logger actlog = new Logger()
                    {
                        type = "Activity@UpdateCard",
                        id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                        description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " deleted a task",
                        published = DateTime.Now,
                        projectId= kanbanUSIn.projectId,
                        data = new Data()
                        {
                            id = kanbanUSIn.Id,
                            name = kanbanUSIn.shortName
                        }
                    };
                    RabbitMQProducer actProducer = new RabbitMQProducer();
                    actProducer.SendMessageToActivityLogger(actlog);

                }
            else if (kanbanUS.tasks.Length < kanbanUSIn.tasks.Length)
                {
                    Logger actlog = new Logger()
                    {
                        type = "Activity@UpdateCard",
                        id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                        description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " added a task",
                        published = DateTime.Now,
                        projectId= kanbanUSIn.projectId,
                        data = new Data()
                        {
                            id = kanbanUSIn.Id,
                            name = kanbanUSIn.shortName
                        }
                    };
                    RabbitMQProducer actProducer = new RabbitMQProducer();
                    actProducer.SendMessageToActivityLogger(actlog);

                }
                // else
                // {
                //     for(var i in kanbanUS.)

                // }
            }



            KanbanData kanbanobj = new KanbanData()
            {
                Id = kanbanUSIn.Id,
                action = "put",
                uniqueId = kanbanUSIn.uniqueId,
                description = kanbanUSIn.description,
                shortName = kanbanUSIn.shortName,
                projectId = kanbanUSIn.projectId,
                userId = kanbanUSIn.userId,
                status = kanbanUSIn.status,
                startTime = kanbanUSIn.startTime,
                endTime = kanbanUSIn.endTime,
                linkedToId = kanbanUSIn.linkedToId,
                assignedTo = kanbanUSIn.assignedTo,
                acceptanceCriteria = kanbanUSIn.acceptanceCriteria,
                points = kanbanUSIn.points,
                tasks = kanbanUSIn.tasks,
                progress = kanbanUSIn.progress
            };

            RabbitMQProducer prod = new RabbitMQProducer();
            
            Console.WriteLine(JsonConvert.SerializeObject(kanbanobj));
            prod.ProduceMessageToGantt(kanbanobj);


            Logger log = new Logger()
            {
                type = "KanBan@UpdateCard",
                id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                description = "Card Updated",
                published = DateTime.Now.Add(new TimeSpan(0,5,30,0)),
                projectId= kanbanUSIn.projectId,
                data = new Data()
                {
                    id = kanbanUSIn.Id,
                    name = kanbanUSIn.shortName
                }
            };
            RabbitMQProducer producer = new RabbitMQProducer();
            producer.SendMessageToLogger(log);
            Console.WriteLine(log);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var kanbanUS = _userService.Get(id);

            if (kanbanUS == null)
            {
                return NotFound();
            }
            // KanbanData kanbanobj = new KanbanData()
            // {
            //     Id = kanbanUS.Id,
            //     action = "delete",
            //     uniqueId = null,
            //     description = kanbanUS.description,
            //     shortName = kanbanUS.shortName,
            //     projectId = kanbanUS.projectId,
            //     userId = kanbanUS.userId,
            //     status = kanbanUS.status,
            //     startTime = kanbanUS.startTime,
            //     endTime = kanbanUS.endTime,
            //     linkedToId = kanbanUS.linkedToId,
            //     assignedTo = kanbanUS.assignedTo,
            //     acceptanceCriteria = kanbanUS.acceptanceCriteria
            // };

            // RabbitMQProducer prod = new RabbitMQProducer();
            // prod.ProduceMessageToGantt(kanbanobj);


            Logger actlog = new Logger()
            {
                type = "Activity@DeleteCard",
                id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                description = _user1Service.Get(HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value).username + " deleted a card named "
                + kanbanUS.shortName,
                published = DateTime.Now,
                projectId= kanbanUS.projectId,
                data = new Data()
                {
                    id = kanbanUS.Id,
                    name = kanbanUS.shortName
                }
            };
            RabbitMQProducer actProducer = new RabbitMQProducer();
            actProducer.SendMessageToActivityLogger(actlog);

            Logger log = new Logger()
            {
                type = "KanBan@DeleteCard",
                id = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value,
                description = "Card deleted",
                published = DateTime.Now,
                projectId= kanbanUS.projectId,
                data = new Data()
                {
                    id = id,
                    name = _userService.Get(id).shortName
                }
            };
            RabbitMQProducer producer = new RabbitMQProducer();
            producer.SendMessageToLogger(log);
            Console.WriteLine(log);
            _userService.Remove(kanbanUS.Id);

            return NoContent();
        }
    }
}