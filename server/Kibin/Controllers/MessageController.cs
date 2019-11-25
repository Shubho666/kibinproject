using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;
using Kibin.SignalR_Hub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IHubContext<ChatHub> _hubContext;

        public MessageController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
    
        // [HttpPost]
        // public async void Post([FromBody]Message msg)
        // {
        //     string retMessage;
        //     try
        //     {
                
            
        //         retMessage = "Success";
        //     }
        //     catch (Exception e)
        //     {
        //         retMessage = e.ToString();
        //     }
        //    // return retMessage;
        // }
        
    }
}