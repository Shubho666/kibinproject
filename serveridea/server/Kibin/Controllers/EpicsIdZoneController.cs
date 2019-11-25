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
    public class EpicsIdZoneController : ControllerBase
    {
        private readonly IEpicIdZoneService _epicsservice;

        public EpicsIdZoneController(IEpicIdZoneService EpicIdZoneService)
        {
            _epicsservice = EpicIdZoneService;
        }

        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<List<EpicsIdZone>> Get() =>
          Ok(_epicsservice.Get());

        [HttpGet("{id:length(24)}", Name = "GetEpicIdZone")]
        [EnableCors("AllowAllHeaders")]

        public ActionResult<EpicsIdZone> Get(string id)
        {
            var epic = _epicsservice.Get(id);

            if (epic == null)
            {
                return NotFound();
            }

            return epic;
        }

        [HttpPost]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Create(EpicsIdZone entity,string username, string userid ,string projectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _epicsservice.Create(entity,username, userid,projectId);

            return CreatedAtRoute("GetEpicIdZone", new { id = entity.Id.ToString() }, entity);
        }

        [HttpPut("{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Update(string id, EpicsIdZone epic)
        {
            var entity = _epicsservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _epicsservice.Update(id, epic);

            return CreatedAtRoute("GetWorkspace", new { id = entity.Id.ToString() }, entity);

        }
        [HttpPut("/EpicsIdZone/{id:length(24)}/addStory")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult UpdateUserStory(string id, string userStory, string username,string story, string userid, string projectId)
        {
            var entity = _epicsservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _epicsservice.UpdateUserStory(id, userStory, username,story, userid, projectId);

            return CreatedAtRoute("GetWorkspace", new { id = entity.Id.ToString() }, entity);

        }

        [HttpPut("/EpicsIdZone/{id:length(24)}/removeStory")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult RemoveUserStory(string id, string userStory, string username, string story, string userid, string projectId)
        {
            var entity = _epicsservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _epicsservice.RemoveUserStory(id, userStory,username ,story, userid, projectId);

            return CreatedAtRoute("GetWorkspace", new { id = entity.Id.ToString() }, entity);

        }

        [HttpPut("/epicname/{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult UpdateEpicName(string id, string newname)
        {
            var entity = _epicsservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _epicsservice.UpdateEpicName(id, newname);

            return CreatedAtRoute("GetWorkspace", new { id = entity.Id.ToString() }, entity);

        }

        [HttpPut("/EpicsIdzone/{id:length(24)}/EpicStatus")]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<EpicsIdZone> UpdateEpicStatus(string id, string status,string username, string userid, string projectId)
        {
            var entity = _epicsservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _epicsservice.UpdateEpicStatus(id, status,username, userid, projectId);

            entity = _epicsservice.Get(id);

            return entity;
         }

        [HttpDelete("{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Delete(string id,string username, string userid, string projectId)
        {
            var entity = _epicsservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _epicsservice.Remove(entity.Id,username, userid, projectId);

            return NoContent();
        }

    }
}
