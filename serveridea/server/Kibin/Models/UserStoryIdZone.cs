using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.Models
{
    public class UserStoryIdZone
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserStoryName { get; set; }

        public string UserStoryType { get; set; }

        public string[] UserStoryDescription { get; set; }

        public string Status { get; set; }

       
    }
}
