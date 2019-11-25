using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kibin.Models;
using Kibin.Services;
using Kibin.RabbitMQ;
using System.Threading;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _tasksService;
        private readonly ILoggerService _loggerService;
        public TasksController(ITaskService TaskService,ILoggerService LoggerService)
        {
            _tasksService = TaskService;
            _loggerService = LoggerService;
        }

       

        [HttpGet]
        public ActionResult<List<Tasks>> Get() =>
            _tasksService.Get();

        [HttpGet("gantt/{id}", Name = "GetTasks")]
        public ActionResult<List<Tasks>> Get(string id)
        {
            var book=_tasksService.GetByProjectId(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }


        [HttpPost]
        public ActionResult<Tasks> Create(Tasks book,string username,string userid,string projectid)
        {

            Thread.Sleep(1000);
            Tasks check= _tasksService.GetByGanttMadeId(book.id);
            
            Console.WriteLine("From Gant Controller" + book.id);
            Console.WriteLine("check{0}",check);
            if(check==null){
                Console.WriteLine("insid null");
            _tasksService.Create(book);
           // Console.WriteLine("Username is",username);

            Tasks obj =new Tasks(){
                TaskId=book.TaskId,
                id=book.id,
                 project_id=book.project_id,
                action="post",
                start_date=book.start_date,
                end_date=book.end_date,
                duration=book.duration,
               progress=book.progress,
               text=book.text,
               
               unique_id=book.unique_id
            };
            RabbitMQProducer producer2 = new RabbitMQProducer();
             producer2.SendUserStoryToBoards(obj);
             Console.WriteLine("created here");            
            // AddUS logger =new AddUS(){
            //     type="Gantt@AddUserStory",
            //     id ="userid",
            //     description=username+" added"+ book.text +"UserStory to gantt chart",
            //     published=DateTime.Now,
            //     data=new Data(){
            //         id=book.id,
            //         name=book.text
            //     }
            // };
            //  RabbitMQProducer producer=new RabbitMQProducer();
            // producer.AddUserStory(logger);

            Logger logger1 =new Logger(){
                type="Activity@AddUserStory",
                id =userid,
                project_id=projectid,
                description=username+" added "+ book.text +" to gantt chart",
                published=DateTime.Now,
                data=new Data(){
                    id=book.id,
                    name=book.text
                }
            };
           // Console.WriteLine(JsonConvert.SerializeObject(logger1));
           // Console.WriteLine(logger1);
             _loggerService.Create(logger1);
            //  RabbitMQProducer producer1=new RabbitMQProducer();
            // producer1.AddUserStoryAct(logger1);

            return CreatedAtRoute("GetTasks", new { id = book.TaskId.ToString() }, book);}
            return Ok();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Tasks bookIn)
        {
            var book = _tasksService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _tasksService.Update(id, bookIn);

            return NoContent();
        }
        
       [HttpPut("gantt/{id}")]
        public IActionResult Update(long id, Tasks1 bookIn,string username,string userid,string projectid)
        {
          var book=_tasksService.PutByGanttId(id,bookIn);
          var book1=_tasksService.GetByGanttId(id);
            if (book1 == null)
            {
                return NotFound();
            }
           
            Tasks obj =new Tasks(){
                TaskId=book1.TaskId,
                id=book1.id,
                 project_id=book1.project_id,
                action="put",
                start_date=book1.start_date,
                end_date=book1.end_date,
                duration=book1.duration,
               progress=book1.progress,
               text=book1.text,
               
               unique_id=book1.unique_id
            };

            RabbitMQProducer producer2 = new RabbitMQProducer();
             producer2.UpdateUserStoryToBoards(obj);

            // UpdUS logger =new UpdUS(){
            //     type="Gantt@UpdateUserStory",
            //     id ="userid",
            //     description=username+" updated"+ bookIn.text +"UserStory in gantt chart",
            //     published=DateTime.Now,
            //     data=new Data(){
            //         id=bookIn.id,
            //         name=bookIn.text
            //     }
            // };
            //  RabbitMQProducer producer=new RabbitMQProducer();
            // producer.UpdateUserStory(logger);

            Logger logger1 =new Logger(){
                published=DateTime.Now,
                type="activity@UpdateUserStory",
                id =userid,
                project_id=projectid,
                description=username+" updated "+ bookIn.text +" in gantt chart",
                data=new Data(){
                    id=bookIn.id,
                    name=bookIn.text
                }
            };
            //Console.WriteLine(JsonConvert.SerializeObject(logger1));
            //Console.WriteLine(logger1);
            _loggerService.Create(logger1);
            //  RabbitMQProducer producer1=new RabbitMQProducer();
            // producer.UpdateUserStoryAct(logger1);


            return NoContent();
           
        }
        

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _tasksService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _tasksService.Remove(book.TaskId);

            return NoContent();
        }

        [HttpDelete("gantt/{id}")]
        public IActionResult Delete(long id,string username,string userid,string projectid)
        {
            var book = _tasksService.GetByGanttId(id);

            if (book == null)
            {
                return NotFound();
            }
            Tasks obj =new Tasks(){
                id=book.id,
                TaskId=book.TaskId,
                 project_id=book.project_id,
                action="delete",
                start_date=book.start_date,
                end_date=book.end_date,
                duration=book.duration,
               progress=book.progress,
               text=book.text,
               unique_id=book.unique_id
            };

            RabbitMQProducer producer2 = new RabbitMQProducer();
             producer2.DeleteUSerStoryFromBoards(obj);


                //Console.WriteLine(JsonConvert.SerializeObject(book));
            _tasksService.RemoveByGanttId(id);

            // DelUS logger =new DelUS(){
            //     type="Gantt@DeleteUserStory",
            //     id ="userid",
            //     description=username+" deleted"+ book.text +"UserStory from gantt chart",
            //     published=DateTime.Now,
            //     data=new Data(){
            //         id=book.id
            //     }
            // };
            //  RabbitMQProducer producer=new RabbitMQProducer();
            // producer.DelUserStory(logger);

            Logger logger1 =new Logger(){
                type="activity@DeleteUserStory",
                id =userid,
                project_id=projectid,
                description=username+" deleted "+ book.text +" from gantt chart",
                published=DateTime.Now,
                data=new Data(){
                    id=book.id
                }
            };
            //Console.WriteLine(JsonConvert.SerializeObject(logger1));
           // Console.WriteLine(logger1);
            _loggerService.Create(logger1);
            //  RabbitMQProducer producer1=new RabbitMQProducer();
            // producer1.DelUserStoryAct(logger1);
            return NoContent();
        }
    }
}
 