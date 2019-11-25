using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


using System;
// using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;

// using Microsoft.AspNetCore.Authorization;
// using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
// using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Kibin.Controllers
{
    [Route("api/[controller]")]
   // [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _userService;

        public ProjectController(IProjectService ProjectService)
        {
            _userService = ProjectService;
        }

        
        [HttpGet]
        // 
        public ActionResult<List<Project>> Get() =>
            Ok(_userService.Get());

        [HttpGet("{id:length(24)}", Name = "GetProject")]
        public ActionResult<Project> Get(string id)
        {
            var book = _userService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("owner/{ownerid}")]
        public ActionResult<List<Project>> GetByOwner(string ownerid)
        {
            

            //var book=new Project();
            var book = _userService.GetByOwner(ownerid);



            if (book == null)
            {
                return NotFound();
            }


            var k=new List<Project>();
            foreach(var i in book){
                //Console.WriteLine(i);
                // if(i.owner==ownerid)
                // {k.Add(i);}
               for(var j1=0;j1<i.owner.Length;j1+=1){
                   if(i.owner[j1]==ownerid){
                    //    Console.WriteLine("inside if");
                       k.Add(i);break;
                   }
               }
                // Console.WriteLine(i.owner.Length);
            }
            foreach( var i in book ){
            //    Console.WriteLine(i);
                for(var j=0;j<i.members.Length;j+=1)
                {
                    if(i.members[j]==ownerid)
                    {
                        k.Add(i);break;
                    }
                }
  
            }
            return k;
        }

        [HttpPost]
        public IActionResult Create(Project book)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _userService.Create(book);

            return CreatedAtRoute("GetProject", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Project bookIn)
        {
            var book = _userService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _userService.Update(id, bookIn);

            return NoContent();
        }

        [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _userService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            var userid=HttpContext.User.Claims.Where(c=>c.Type=="id").FirstOrDefault().Value;
            Project project=_userService.Get(id);
            Console.WriteLine(userid);
            Console.WriteLine(project.owner);
            bool flag=false;
            for(int i=0;i<project.owner.Length;i++){
                if(userid==project.owner[i]){
                    flag=true;break;
                }
            }
            if(flag==true){
                _userService.Remove(book.Id);
            }
            else{
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            

            return NoContent();
        }
    }
}