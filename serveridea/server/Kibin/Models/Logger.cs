using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kibin.Models
{

    public class Logger_Domain
    {
        public string description { get; set; }
        public string type { get; set; }
        public DateTime published { get; set; }
    }
    public class Logger_Activity
    {
        public string id { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public data details { get; set; }
        public DateTime published { get; set; }
        public string projectId { get; set; }
    }
    public class data
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}
