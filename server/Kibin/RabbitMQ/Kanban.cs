using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Kibin.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.RabbitMQ
{
    public class KanbanData
    {
        
        public string Id { get; set; }
        public string action{get;set;}
        public string description {get;set;}

        public string shortName {get;set;}

        public string projectId {get;set;}
        public string userId {get;set;}
        public AssignedTo[] assignedTo{get;set;}
        public string status {get;set;}
        public DateTime startTime{get;set;}
        public DateTime endTime{get;set;}
        public string linkedToId{get;set;}
        // public string[] assignedTo{get;set;}

        public string[] acceptanceCriteria {get;set;}

        public USTask[] tasks {get;set;}
        public int points{get;set;}

        public string epicId {get;set;}

        public string epicTitle{get;set;}
        public double progress{get;set;}
        public string uniqueId {get;set;}
    }

    public class Listrabbit
    {
        public string name {get;set;}
        public UserStory[] UserStory{get;set;}
        public string projectId {get;set;}
        public int index{get;set;}
    }
    //public class Subtask
    //{
    //    public string subtaskId {get;set;}
    //    public string subtaskStatus {get;set;}
    //    public string subtaskDescription {get;set;}
    //}

    //public class USTask{
    //    public string taskId{get;set;}
    //    public string taskName{get;set;}
    //    public string[] assigneeId{get;set;}
    //    public Subtask[] subtask{get;set;}
    //}
    
    public class Tasks11
    {
        

        public long id {get; set;}

        public long start_date { get; set; }

        public string text { get; set; }

        public decimal progress { get; set; }

        public int duration { get; set; }
        public long end_date {get; set;}
    }

    
}
    