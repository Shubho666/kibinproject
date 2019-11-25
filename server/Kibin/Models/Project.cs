using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using System.DateTime;
using System;

namespace Kibin.Models
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get;set;}
        public string projectName {get;set;}
        public string projectType {get;set;}
        public string projectDescription {get;set;}

        public DateTime startTime{get;set;}
        public DateTime endTime {get;set;}

        public string[] owner {get;set;}
        public string[] members {get;set;}
    }

    
}