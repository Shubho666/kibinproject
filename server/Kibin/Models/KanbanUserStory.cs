using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Kibin.Models
{
    public class KanbanUserStory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string description {get;set;}

        public string shortName {get;set;}

        public string projectId {get;set;}
        public string userId {get;set;}

        public string status {get;set;}
        public DateTime startTime{get;set;}
        public DateTime endTime{get;set;}
        public string linkedToId{get;set;}
        public AssignedTo[] assignedTo{get;set;}

        public string[] acceptanceCriteria {get;set;}

        public USTask[] tasks {get;set;}
        public int points{get;set;}

        public string epicId {get;set;}

        public string epicTitle{get;set;}
        public double progress{get;set;}
        public string uniqueId {get;set;}

    }

    public class Subtask
    {
        public string subtaskId {get;set;}
        public string subtaskStatus {get;set;}
        public string subtaskDescription {get;set;}
    }

    public class USTask{
        public string taskId{get;set;}
        public string taskName{get;set;}
        public string[] assigneeId{get;set;}
        public Subtask[] subtask{get;set;}
    }
    public class AssignedTo{
       public string assignedToId{get;set;}
       public string assignedToName{get;set;}
   }
}