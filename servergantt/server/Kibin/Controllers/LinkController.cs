using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kibin.RabbitMQ;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly LinkService _linkService;
        private readonly LoggerService _loggerService;

        public LinkController(LinkService LinkService,LoggerService LoggerService)
        {
            _linkService = LinkService;
            _loggerService = LoggerService;
        }

        [HttpGet]
        public ActionResult<List<Link>> Get() =>
            _linkService.Get();

        [HttpGet("gantt/{id}", Name = "GetLink")]
        public ActionResult<List<Link>> Get(string id)
        {
            var book=_linkService.GetByProjectId(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Link> Create(Link book,string username,string userid,string projectid)
        {
            _linkService.Create(book);

            // AddLink logger =new AddLink(){
            //     type="Gantt@AddDependency",
            //     id ="userid",
            //     description=username+" added a dependency to gantt chart",
            //     published=DateTime.Now,
            //     data=new Data(){
            //         id=book.id
            //     }
            // };
            //  RabbitMQProducer producer=new RabbitMQProducer();
            // producer.AddLink(logger);

            Logger logger1 =new Logger(){
                type="activity@AddDependency",
                id =userid,
                project_id=projectid,
                description=username+" added a dependency to gantt chart",
                published=DateTime.Now,
                data=new Data(){
                    id=book.id
                }
            };
            //Console.WriteLine(JsonConvert.SerializeObject(logger1));
           // Console.WriteLine(logger1);
            _loggerService.Create(logger1);
            //  RabbitMQProducer producer1=new RabbitMQProducer();
            // producer1.AddLinkAct(logger1);


            return CreatedAtRoute("GetLink", new { id = book.LinkId.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Link bookIn)
        {
            var book = _linkService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _linkService.Update(id, bookIn);

            return NoContent();
        }

        [HttpPut("gantt/{id}")]
        public IActionResult Update(long id, Link bookIn,string username)
        {
            var book = _linkService.PutByGanttId(id,bookIn);

            if (book == null)
            {
                return NotFound();
            }


            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _linkService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _linkService.Remove(book.LinkId);

            

            return NoContent();
        }

        [HttpDelete("gantt/{id}")]
        public IActionResult Delete(long id,string username,string userid,string projectid)
        {
            var book = _linkService.GetByGanttId(id);

            if (book == null)
            {
                return NotFound();
            }

            _linkService.RemoveByGanttId(id);

            // DelLink logger =new DelLink(){
            //     type="Gantt@DeleteDependency",
            //     id ="userid",
            //     description=username+" deleted a dependency from gantt chart",
            //     published=DateTime.Now,
            //     data=new Data(){
            //         id=book.id
            //     }
            // };
            //  RabbitMQProducer producer=new RabbitMQProducer();
            // producer.DelLink(logger);

            Logger logger1 =new Logger(){
                type="activity@AddUserStory",
                id =userid,
                project_id=projectid,
                description=username+" deleted a dependency from gantt chart",
                published=DateTime.Now,
                data=new Data(){
                    id=book.id
                }
            };
            //Console.WriteLine(JsonConvert.SerializeObject(logger1));
           // Console.WriteLine(logger1);
            _loggerService.Create(logger1);
            //  RabbitMQProducer producer1=new RabbitMQProducer();
            // producer1.DelLinkAct(logger1);


            return NoContent();
        }
    }
}