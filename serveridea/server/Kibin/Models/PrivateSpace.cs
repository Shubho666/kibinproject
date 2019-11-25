using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.Models
{
    public class PrivateSpace
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }

        public ProjectSpecificEpics[] Pse { get; set; }
    }
    public class ProjectSpecificEpics
    {
        public string ProjectId { get; set; }
        public string[] Epics { get; set; }
    }

}
