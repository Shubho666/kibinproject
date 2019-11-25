using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kibin.Models
{
    public class Message
    {
        public UserStoryClass UserStory { get; set; }
        public string name {get;set;}
    }
    public class UserStoryClass
    {
        public string UserStoryId {get;set;}
        public string UserStoryName {get;set;}
    }
    public class MessageList
    {
        public string sourceName {get;set;}
        public string destinationName {get;set;}
        public UserStoryClass[] sourceList {get;set;}
        public UserStoryClass[] destinationList {get;set;}
    }

    public class ModalDetails
    {
        public string id{get;set;}
        public string usname{get;set;}
        public int points{get;set;}
        public string[] acceptanceCriteria{get;set;}
        public ModalTaskDetails[] modalTasks {get;set;}
        public ModalAssignedTo[] assignedTo{get;set;}
        public string shortName{get;set;}

    }
    public class ModalTaskDetails
    {
        public string taskId{get;set;}
        public string taskName{get;set;}
        public string[] assigneeId{get;set;}
        public ModalSubtask[] subtask {get;set;}
    }

    public class ModalSubtask{
        public string subtaskId{get;set;}
        public string subtaskStatus{get;set;}
        public string subtaskDescription{get;set;}
    }

    public class ModalAssignedTo{
        public string assignedToId{get;set;}
        public string assignedToName{get;set;}
    }
    public class NewColumn
    {
        public string id {get;set;}
        public string name {get;set;}
        public string projectId {get;set;}
        public UserStoryClass[] userStory {get;set;}
        public int index{get;set;}
    }
    public class ListForEdit{
        public string id{get;set;}
        public string listName{get;set;}
    }
    public class AllList{
        
        public string id {get;set;}
        public string name {get;set;}
        public string projectId {get;set;}
        public int index {get;set;}
        public UserStoryClass[] userStory {get;set;}
    }

    public class AllProject{
        public string id {get;set;}
        public string projectName {get;set;}
        public string projectType {get;set;}
        public string projectDescription {get;set;}
        public DateTime startTime {get;set;}
        public DateTime endTime {get;set;}
        public string owner {get;set;}
        public string[] members {get;set;}
    }

    public class BoardListUpdate{
        public string listId{get;set;}
        public string listName{get;set;}
        public string userStoryId{get;set;}
        public string userStoryName{get;set;}
    }
    public class UpdateMembers{
        public string memberId{get;set;}
        public bool status{get;set;}
        public string img{get;set;}
    }

    public class TransferMembers{
        public string memberId{get;set;}
        public bool status{get;set;}
        public string projectId{get;set;}
        public string[] members{get;set;}
        public string[] owner{get;set;}
    }
}
