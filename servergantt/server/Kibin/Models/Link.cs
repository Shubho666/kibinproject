using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.Models
{
    public class Link
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LinkId { get; set; }
        public long id {get;set;}

        public string project_id {get;set;}
        public string source { get; set; }
        
        public string target { get; set; }

        public string type { get; set; }
    }
    
    public class Link1{
        public long id {get;set;}
        public string source { get; set; }
        
        public string target { get; set; }

        public string type { get; set; }
    }
}
