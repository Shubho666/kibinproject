using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly LoggerService _loggerService;

        public LoggerController(LoggerService UsersService)
        {
            _loggerService = UsersService;
        }

        [HttpGet]
        public ActionResult<List<Logger>> Get() =>
            _loggerService.Get();

        [HttpGet("project/{projectid}/user_id/{userid}", Name = "Logger")]
        public ActionResult<List<Logger>> Get(string projectid,string userid)
        {
            var book = _loggerService.GetByProjectandUser( projectid, userid);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }
        [HttpGet("project/{projectid}")]
        public ActionResult<List<Logger>> Get(string projectid)
        {
            var book = _loggerService.GetByProject(projectid);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpPost]
        public ActionResult<Logger> Create(Logger book)
        {
            _loggerService.Create(book);

            return CreatedAtRoute("GetLogger", new { id = book.LoggerId.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Logger bookIn)
        {
            var book = _loggerService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _loggerService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _loggerService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _loggerService.Remove(book.LoggerId);

            return NoContent();
        }
    }
}