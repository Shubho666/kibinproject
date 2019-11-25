using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Kibin.Models;

namespace Kibin.Hubs
{
    public class GanttHub : Hub
    {
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public Task SendMessageToGroup(string groupName, Tasks message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
        public Task UpdateTask(string groupName, Tasks1 message){
            return Clients.Group(groupName).SendAsync("TaskUpdated",message);
        }

        public Task AddTask(string groupName, Tasks1 message){
            return Clients.Group(groupName).SendAsync("TaskAdded",message);
        }

        public Task DeleteTask(string groupName, long message){
            return Clients.Group(groupName).SendAsync("TaskDeleted",message);
        }

        public Task AddLink(string groupName,Link1 message){
            return Clients.Group(groupName).SendAsync("LinkAdded",message);
        }

        public Task DeleteLink(string groupName,long message){
            return Clients.Group(groupName).SendAsync("LinkDeleted",message);
        }

        public Task UpdateLink(string groupName,long message){
            return Clients.Group(groupName).SendAsync("LinkUpdated",message);
        }
     
    }
}