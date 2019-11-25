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
    public class UserStoryIdeaZoneController : ControllerBase
    {
        private readonly IUserStoryIdZoneService _usservice;

        public UserStoryIdeaZoneController(IUserStoryIdZoneService UserStoryIdZoneService)
        {
            _usservice = UserStoryIdZoneService;
        }

        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<List<UserStoryIdZone>> Get() =>
          Ok(_usservice.Get());


        [HttpGet("{id:length(24)}", Name = "GetUserStoryIdZone")]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<UserStoryIdZone> Get(string id)
        {
            var usStory = _usservice.Get(id);

            if (usStory == null)
            {
                return NotFound();
            }

            return usStory;
        }

        [HttpPost]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Create(UserStoryIdZone entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _usservice.Create(entity);

            return CreatedAtRoute("GetUserStoryIdZone", new { id = entity.Id.ToString() }, entity);
        }

        [HttpPut("{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Update(string id, UserStoryIdZone usStory)
        {
            var entity = _usservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _usservice.Update(id, usStory);

            return CreatedAtRoute("GetUserStoryIdZone", new { id = entity.Id.ToString() }, entity);

        }

        [HttpPut("/userstoryDescription/{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult UpdateUserStoryDescription(string id, string[] usStory)
        {
            var entity = _usservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _usservice.UpdateUserStoryDescription(id, usStory);

            return CreatedAtRoute("GetUserStoryIdZone", new { id = entity.Id.ToString() }, entity);

        }
        [HttpPut("/userstoryStatus/{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult UpdateUserStoryStatus(string id, string status, string ProjectId,string username, string userid)
        {
            var entity = _usservice.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            _usservice.UpdateStoryStatus(id, status, ProjectId, username,userid);

            return CreatedAtRoute("GetUserStoryIdZone", new { id = entity.Id.ToString() }, entity);
        }

        [HttpPut("/userstoryType/{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult UpdateUserStoryType(string id,string type)
        {
            var entity = _usservice.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            _usservice.UpdateStoryType(id, type);

            return CreatedAtRoute("GetUserStoryIdZone", new { id = entity.Id.ToString() }, entity);
        }

        [HttpPut("/userstoryName/{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult UpdateUserStoryName(string id, string newname)
        {
            var entity = _usservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _usservice.UpdateUserStoryName(id, newname);

            return CreatedAtRoute("GetUserStoryIdZone", new { id = entity.Id.ToString() }, entity);

        }

        [HttpDelete("{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Delete(string id)
        {
            var entity = _usservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _usservice.Remove(entity.Id);

            return NoContent();
        }
    }
}
