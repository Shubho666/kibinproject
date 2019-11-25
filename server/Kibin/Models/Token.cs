using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kibin.Models {
    public class Token {
        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }
        public string appName{get;set;}
        public DateTime expiryDate{get;set;} 
        public string token{get;set;}
        public string userId{get;set;}
       
    }

    
}