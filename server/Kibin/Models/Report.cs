using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.Models
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string project_id { get; set; }

        
        public List<UserStories> list {get;set;}
    }
    public class UserStories{

        public string id ;
        public string label{get;set;}
        public List<string> userstoryid{get;set;}
        public DateTime date{get;set;}
    }






}