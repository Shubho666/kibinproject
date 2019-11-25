using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Kibin.Models
{
    public class EpicMessage
    {
        public string EpicId { get; set; }
        public string Epic { get; set; }
        public string status { get; set; }
        public string[] userStoriesIds { get; set; }
        public string[] userstories { get; set; }

        public string[] userstoryStatuses { get; set; }

        public bool[] userstorychecked { get; set; }
    }
    public class UserStoryMessage
    {
        public string epic{get; set;}
        public int index {get; set;}
        public string epicId {get; set;}
        public string sindex{get;set;}
        public string addeduserstory {get; set;}
        public string addeduserstoryId{get; set;}
    }
    public class ModalMessage
    {
      public string epicId{get; set;}
      public int  storyIndex {get; set;} 
      public string  storyValue {get; set;}
      public bool  flag {get; set;}
    }
}