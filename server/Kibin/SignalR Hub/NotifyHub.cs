using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Kibin.Models;


namespace Kibin.SignalR_Hub
{
    //public class NotifyHub : Hub<ITypedHubClient>
    //public class NotifyHub: Hub
    //{
        //Task BroadCastMessage (Message msg)
        // public Task JoinGroup(string group) 
        // {
        //     return Groups.AddToGroupAsync(Context.ConnectionId, group);
        // }

        // public Task SendMessageToGroup(string group, string message)
        // {
        //     return Clients.Group(group).SendAsync("ReceiveMessage", message);
        // }
        
    //}
    public class ChatHub : Hub
    {
         public System.Threading.Tasks.Task JoinGroup(string groupName){
            return Groups.AddToGroupAsync(Context.ConnectionId,groupName);
        }
        public System.Threading.Tasks.Task SendMessageToGroup(string groupName,Message message){
            return Clients.Group(groupName).SendAsync("ReceiveMessage",message);
        }
         public System.Threading.Tasks.Task SendMessageToGroupForUsEdit(string groupName,Message message){
            return Clients.Group(groupName).SendAsync("ReceiveMessageEditedUs",message);
        }
        public System.Threading.Tasks.Task SendMessageToGroupForListNameEdit(string groupName,ListForEdit message){
            return Clients.Group(groupName).SendAsync("ReceiveMessageEditedListName",message);
        }
        public System.Threading.Tasks.Task SendMessageListToGroup(string groupName,MessageList messagelist){
            return Clients.Group(groupName).SendAsync("ReceiveMessageList",messagelist);
        }
        public System.Threading.Tasks.Task SendMessageToGroupForDelete(string usid,ModalDetails modaldetails){
            return Clients.Group(usid).SendAsync("ReceiveMessageDelete",modaldetails);
        }
        public System.Threading.Tasks.Task SendModalDetailsToUsId(string usid,ModalDetails modaldetails)
        {
            return Clients.Group(usid).SendAsync("ReceiveModalDetails",modaldetails);
        }
        public System.Threading.Tasks.Task SendNewColumnName(string usid,NewColumn columnName)
        {
            return Clients.Group(usid).SendAsync("ReceiveColumnName",columnName);
        }
        public System.Threading.Tasks.Task SendColumnToDelete(string usid,string columnName)
        {
            return Clients.Group(usid).SendAsync("ReceiveColumnToDelete",columnName);
        }
        public System.Threading.Tasks.Task SendUpdatedPositionOfColumns(string boardid,AllList[] allList)
        {
            return Clients.Group(boardid).SendAsync("ReceiveUpdatedList",allList);
        }
        public System.Threading.Tasks.Task SendAllProject(string dashboardid,AllProject[] allProject)
        {
            return Clients.Group(dashboardid).SendAsync("ReceiveAllProject",allProject);
        }
        public System.Threading.Tasks.Task SendFromModalToBoard(string dashboardid,BoardListUpdate boardListUpdate)
        {
            return Clients.Group(dashboardid).SendAsync("ReceiveFromModalToBoard",boardListUpdate);
        }
        // for signalr and rabbitmq

        public void SendUserStoryToAddOnList (string boardid,string msg)
        {
            Clients.Group(boardid).SendAsync("RabbitListUpdate",msg);
        }

        public void SendGanttToUpdate (string boardid)
        {
            Clients.Group(boardid).SendAsync("RabbitGanttUpdate");
        }

        public System.Threading.Tasks.Task SendForAddMembers(string projectid,UpdateMembers updateMembers)
        {
            return Clients.Group(projectid).SendAsync("ReceiveForAddMembers",updateMembers);
        }
        public System.Threading.Tasks.Task SendForRemoveMembers(string projectid,UpdateMembers updateMembers)
        {
            return Clients.Group(projectid).SendAsync("ReceiveForRemoveMembers",updateMembers);
        }
        public System.Threading.Tasks.Task SendForTransferMembers(string projectid,TransferMembers transferMembers)
        {
            return Clients.Group(projectid).SendAsync("ReceiveForTransferMembers",transferMembers);
        }

        // public async Task AddToGroup(string groupName)
        // {
        //     await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        //     await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        // }

        // public async Task RemoveFromGroup(string groupName)
        // {
        //     await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        //     await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        // }

        // public Task SendPrivateMessage(string user, string message)
        // {
        //     return Clients.User(user).SendAsync("ReceiveMessage", message);
        // }
    }
}
