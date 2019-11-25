using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LoggerActivityController : ControllerBase
    {
        private readonly LoggerActivityService _loggerService;

        public LoggerActivityController(LoggerActivityService loggerService)
        {
            _loggerService = loggerService;
        }
        [HttpGet]
        public ActionResult<List<LoggerActivity>> Get() =>
            _loggerService.Get();

        [HttpGet("{id:length(24)}", Name = "GetLoggerActivity")]
        public ActionResult<LoggerActivity> Get(string id)
        {
            var book = _loggerService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("userid/{userid}")]
        public ActionResult<List<LoggerActivity>> GetByUserId(string userid)
        {
            var book = _loggerService.GetByUserId(userid);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("projectid/{projectid}")]
        public ActionResult<List<LoggerActivity>> GetByProjectId(string projectid)
        {
            var book = _loggerService.GetByProjectId(projectid);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("userid/projectid/{userid}/{projectid}")]
        public ActionResult<List<LoggerActivity>> GetByProjectAndUser(string userid, string projectid)
        {
            var book = _loggerService.GetByProjectAndUser(userid, projectid);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public IActionResult Create(LoggerActivity book)
        {
            _loggerService.Create(book);

            return CreatedAtRoute("GetLoggerActivity", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, LoggerActivity bookIn)
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

            _loggerService.Remove(book.Id);

            return NoContent();
        }
        [HttpDelete("all/projectid/{projectid}")]
        public IActionResult DeleteAll(string projectid)
        {
            var book = _loggerService.GetByProjectId(projectid);

            if (book == null)
            {
                return NotFound();
            }

            _loggerService.RemoveAll(book[0].projectId);

            return NoContent();
        }
    }
}