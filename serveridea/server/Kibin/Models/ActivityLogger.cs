using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using System.DateTime;
using System;
using Kibin.RabbitMQ;

namespace Kibin.Models
{
    public class LoggerActivity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string type { get; set; }

        public string userid { get; set; }    //    UserId
        public DateTime published { get; set; }
        public string description { get; set; }
        public data data { get; set; }
        public string projectId { get; set; }
    }


}