using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    public class PrivateSpaceController : ControllerBase
    {
        private readonly IPrivateSpaceService _privateSpaceservice;

        public PrivateSpaceController(IPrivateSpaceService privateSpaceService)
        {
            _privateSpaceservice = privateSpaceService;
        }


        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<List<PrivateSpace>> Get() =>
           Ok(_privateSpaceservice.Get());

        [HttpGet("{id:length(24)}", Name = "GetPrivatespace")]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<PrivateSpace> Get(string id)
        {
            var user = _privateSpaceservice.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;

        }


        [HttpGet("/PrivateSpace/user/{id}", Name = "GetPrivateUserspace")]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<PrivateSpace> GetByUserId(string id)
        {
            var user = _privateSpaceservice.GetByUserId(id);

            if (user == null)
            {
                PrivateSpace obj = new PrivateSpace();
                obj.UserId = "No-such-user";
                return obj;
            }

            return user;

        }


        [HttpPost]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Create(PrivateSpace entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _privateSpaceservice.Create(entity);

            return CreatedAtRoute("GetPrivatespace", new { id = entity.Id.ToString() }, entity);
        }

        [HttpPut("{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Update(string id, PrivateSpace entity)
        {
            var user = _privateSpaceservice.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _privateSpaceservice.Update(id, entity);

            return CreatedAtRoute("GetPrivatespace", new { id = entity.Id.ToString() }, entity);

        }

        [HttpPut("/PrivateSpace/user/{id}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult UpdateByUserId(string id, PrivateSpace entity)
        {
            var user = _privateSpaceservice.GetByUserId(id);

            if (user == null)
            {
                return NotFound();
            }
            _privateSpaceservice.Updatebyid(id, entity);

            return CreatedAtRoute("GetPrivatespace", new { id = entity.Id.ToString() }, entity);

        }


        [HttpDelete("{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Delete(string id)
        {
            var entity = _privateSpaceservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _privateSpaceservice.Remove(entity.Id);

            return NoContent();
        }












    }
}
