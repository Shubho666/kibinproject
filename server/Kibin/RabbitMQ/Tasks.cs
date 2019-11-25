using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.RabbitMQ
{
    public class Tasks
    {
        
        public string TaskId { get; set; }

        public long id {get; set;}
        public string project_id {get;set;}
        public string unique_id {get;set;}

        public long start_date { get; set; }

        public string text { get; set; }

        public double progress { get; set; }

        public int duration { get; set; }
        public long end_date {get; set;}
        public string action {get;set;}
    }
    public class Tasks1
    {
        

        public long id {get; set;}

        public long start_date { get; set; }

        public string text { get; set; }

        public decimal progress { get; set; }

        public int duration { get; set; }
        public long end_date {get; set;}
    }

    
}
    