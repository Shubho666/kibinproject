using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Kibin.Models
{

      public class Logger {
         [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LoggerId { get; set; }
          public DateTime published;
         public string type;
        public string id;

         public string project_id;
        public string description;

        public Data data;
      }
   //   public class AddUS{
   //       public DateTime published;
   //       public string type;

   //       public string project_id;
   //      public string description;
   //      public string id;

   //      public Data data;
   //   }

   //   public class DelUS{
   //       public DateTime published;
   //       public string type;
   //      public string description;
   //      public string id;

   //      public Data data;
   //   }

   //   public class UpdUS{
   //        public DateTime published;
   //       public string type;
   //      public string description;
   //      public string id;

   //      public Data data;
   //   }

   //   public class AddLink{
   //       public DateTime published;
   //       public string type;
   //      public string description;
   //      public string id;

   //      public Data data;
   //   }

   //   public class DelLink{
   //       public DateTime published;
   //       public string type;
   //      public string description;
   //      public string id;

   //      public Data data;
   //   }
       
       public class Data{
          public long id ;

          public string name;
       } 

}