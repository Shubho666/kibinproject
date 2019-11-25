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
    public class EpicController: ControllerBase
    {
        private readonly EpicService _epicService;

        public EpicController(EpicService EpicService)
        {
            _epicService = EpicService;
        }
        [HttpGet]
        public ActionResult<List<Epic>> Get() =>
            _epicService.Get();

        [HttpGet("{id:length(24)}", Name = "GetEpic")]
        public ActionResult<Epic> Get(string id)
        {
            var book = _epicService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public IActionResult Create(Epic book)
        {
            _epicService.Create(book);

            return CreatedAtRoute("GetEpic", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Epic bookIn)
        {
            var book = _epicService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _epicService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _epicService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _epicService.Remove(book.Id);

            return NoContent();
        }
    }
}