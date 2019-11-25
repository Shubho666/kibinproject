using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Kibin.Models;
namespace Kibin.SignalR_Hub
{
    public class ColHub : Hub
    {
        public System.Threading.Tasks.Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public System.Threading.Tasks.Task SendMessageToGroup(string groupName, EpicMessage message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
        public System.Threading.Tasks.Task SendMessageToGroupD(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveMessageD", message);
        }
        public System.Threading.Tasks.Task SendMessageToGroupAU(string groupName, UserStoryMessage message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveMessageAU", message);
        }
        public System.Threading.Tasks.Task SendMessageToGroupM(string groupName, ModalMessage message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveMessageM", message);
        }
    }
}

