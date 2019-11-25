using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kibin.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GanttHubController : ControllerBase
    {
        private IHubContext<GanttHub> _hubContext;
        public GanttHubController(IHubContext<GanttHub> hubContext)
        {
            _hubContext = hubContext;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("values")]
        public async void Post([FromBody] string value)
        {
            await _hubContext.Clients.User(value).SendAsync("Success");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}