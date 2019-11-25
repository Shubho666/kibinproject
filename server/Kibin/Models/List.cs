using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.Models
{
    public class List
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string name {get;set;}
        public UserStory[] UserStory{get;set;}
        public string projectId {get;set;}
        public int index{get;set;}
    }
    public class UserStory{
      public string UserStoryId{get;set;}
      public string UserStoryName{get;set;}
    }
}