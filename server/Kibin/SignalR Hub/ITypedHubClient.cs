using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kibin.Models;

namespace Kibin.SignalR_Hub
{
    
    
        public interface ITypedHubClient
        {
             System.Threading.Tasks.Task BroadcastMessage(Message msg);

             //System.Threading.Tasks.Task JoinGroup(Message msg) ;
        

            //  System.Threading.Tasks.Task SendMessageToGroup(string group, string message);
        
    
        }
    
}

