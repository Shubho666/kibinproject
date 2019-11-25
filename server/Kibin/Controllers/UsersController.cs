using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _userService;

        public UsersController(UsersService UsersService)
        {
            _userService = UsersService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() =>
            _userService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            Console.WriteLine(id);
            var book = _userService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("{name}")]
        public ActionResult<User> GetByUsername(string name)
        {
            Console.WriteLine(name);
            var book = _userService.GetByUsername(name);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<User> Create(User book)
        {
            _userService.Create(book);

            return CreatedAtRoute("GetUser", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User bookIn)
        {
            var book = _userService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _userService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _userService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _userService.Remove(book.Id);

            return NoContent();
        }
    }
}