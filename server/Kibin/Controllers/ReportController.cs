using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
  //  [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;
        public BsonDocument _filter;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public ActionResult<List<Report>> Get() =>
            _reportService.Get();

        [HttpGet("{id:length(24)}", Name = "GetReport")]
        public ActionResult<Report> Get(string id)
        {
            Console.WriteLine(id);
            var book = _reportService.GetbyId(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("project/{projectid}")]
        public ActionResult<Report> GetByProjectId(string projectid)
        {
            Console.WriteLine(projectid);
            var book = _reportService.GetByProjectId(projectid);

            if (book == null)
            {
                return NotFound();
            }


            return book;
        }
        [HttpGet("project/{projectid}/{date}")]
        public  Report GetByProjectIdandDate(string projectid,DateTime date)
        {
            Console.WriteLine(projectid);
            var book = _reportService.GetByProjectIdandDate(projectid,date);

            // if (book == null)
            // {
            //     return NotFound();
            // }


            return book;
        }

     

        [HttpPost]
        public ActionResult<Report> Create(Report book)
        {   
            _reportService.Create(book);

            return CreatedAtRoute("GetReport", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Report bookIn)
        {
            var book= Get(id);
            if(book==null)
         {   Console.WriteLine("No updateMade");
            return NoContent();

        }
         var response = _reportService.Update(id,bookIn.list);
         Console.WriteLine(bookIn.list);
         Console.WriteLine("OK");
         return Ok(response);


        }

        [HttpDelete("{id:lengtH(24)}")]
        public IActionResult Delete(string id)
        {
            var book =_reportService.GetbyId(id);

            if (book == null)
            {
                return NotFound();
            }

           _reportService.Remove(book.Id);

            return NoContent();
        }
    }
}