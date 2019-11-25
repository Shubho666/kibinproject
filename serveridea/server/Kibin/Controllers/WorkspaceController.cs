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
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkSpaceService _workspaceservice;

        public WorkspaceController(IWorkSpaceService WorkSpaceService)
        {
            _workspaceservice = WorkSpaceService;
        }

        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<List<WorkSpace>> Get() =>
           Ok(_workspaceservice.Get());

        [HttpGet("{id:length(24)}", Name = "GetWorkspace")]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<WorkSpace> Get(string id)
        {
            var project = _workspaceservice.Get(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }


        [HttpGet("/workspace/{ProjectId}", Name = "GetWorkspaceById")]
        [EnableCors("AllowAllHeaders")]
        public ActionResult<WorkSpace> GetByProjectId(string ProjectId)
        {
            var project = _workspaceservice.GetByProjectId(ProjectId);

            if (project == null)
            {
                WorkSpace obj = new WorkSpace();
                obj.ProjectId = "";
                return obj;
            }

            return project;
        }







        //[HttpGet("userid/{userid}")]
        //public ActionResult<KanbanUserStory> GetUserId(string userid)
        //{
        //    var book = _userService.GetUserId(userid);

        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return book;
        //}

        [HttpPost]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Create(WorkSpace entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _workspaceservice.Create(entity);

            return CreatedAtRoute("GetWorkspace", new { id = entity.Id.ToString() }, entity);
        }

        [HttpPut("{projectId}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Update(string projectId, string epicid)
        {
            var entity = _workspaceservice.GetByProjectId(projectId);

            if (entity == null)
            {
                return NotFound();
            }

            _workspaceservice.Update(projectId, epicid);

            return CreatedAtRoute("GetWorkspace", new { id = entity.Id.ToString() }, entity);
            
        }

        [HttpPut("/Workspace/{projectId}/epic/{epicid}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult DeleteEpic(string projectId, string epicid)
        {
            var entity = _workspaceservice.GetByProjectId(projectId);

            if (entity == null)
            {
                return NotFound();
            }

            _workspaceservice.DeleteEpic(projectId, epicid);
            return CreatedAtRoute("GetWorkspace", new { id = entity.Id.ToString() }, entity);

        }

        [HttpDelete("{id:length(24)}")]
        [EnableCors("AllowAllHeaders")]
        public IActionResult Delete(string id)
        {
            var entity = _workspaceservice.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _workspaceservice.Remove(entity.Id);

            return NoContent();
        }
    }
}
