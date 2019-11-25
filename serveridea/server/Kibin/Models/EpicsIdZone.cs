using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.Models
{
    public class EpicsIdZone
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int EpicId { get; set; }
        public string EpicName { get; set; }
        public string status { get; set; }
        public string[]  UserStories { get; set; }

        public string[] userstoryStatuses { get; set; }

        public bool[] userstorychecked { get; set; }

    }
}
