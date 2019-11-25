using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Kibin.Models {
    public class User {
        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string accessToken { get; set; }
        public string accessTokenForSwagge{get;set;}
        public string avatar_url { get; set; }
        public bool admin{get;set;}
        public ProjectDetails[] projectDetails { get; set; }
    }

    public class ProjectDetails {
        public string projectId { get; set; }
        public string role { get; set; }
    }
    public class GitlabUserResponse {
      public int id{get;set;}
      public string name{get;set;}
      public string username{get;set;}
      public string state{get;set;}
      public string avatar_url{get;set;}
      public string web_url{get;set;}
      public DateTime created_at{get;set;}
      public string bio{get;set;}
      public string location{get;set;}
      public string public_email{get;set;}
      public string skype{get;set;}
      public string linkedin{get;set;}
      public string twitter{get;set;}
      public string website_url{get;set;}
      public string organization{get;set;}
      public DateTime last_sign_in_at{get;set;}
      public DateTime confirmed_at{get;set;}
      public DateTime last_activity_on{get;set;}
      public string email{get;set;}
      public int theme_id{get;set;}
      public int color_scheme_id{get;set;}
      public int projects_limit{get;set;}
      public DateTime current_sign_in_at{get;set;}
      public string[] identities{get;set;}
      public bool can_create_group{get;set;}
      public bool can_create_project{get;set;}
      public bool two_factor_enabled{get;set;}
      public bool external{get;set;}
      public string private_profile{get;set;}
    }
}