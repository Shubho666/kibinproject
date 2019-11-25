using System;
using Kibin.Models;
namespace Kibin.RabbitMQ
{
    
   public class DeleteLogger
   {
       public string @context{get;set;}
       public string id{get;set;}
       public string summary{get;set;}
       public string type{get;set;}
       public string objects{get;set;}
        public Actor actor{get;set;}
        public Origin origin{get;set;}
   }
   public class Actor{
       public string type{get;set;}
       public string name{get;set;}
   }
   public class Origin
   {
       public string type{get;set;}
       public string name{get;set;}
   }
   public class Target
   {
       public string type{get;set;}
       public string name{get;set;}
   }

   public class CreateLogger
   {
       public string type{get;set;}
    
       public string id{get;set;}    //    UserId
       public DateTime published{get;set;}
       public string description{get;set;}
       public Data data{get;set;}


    //    public Actor data{get;set;}

   }
    public class Logger
   {
       public string type{get;set;}
    
       public string id{get;set;}    //    UserId
       public DateTime published{get;set;}
       public string description{get;set;}
       public Data data{get;set;}
       public string projectId{get;set;}

    //    public Actor data{get;set;}

   }
   public class Data{

       public string id{get;set;}
       public string name{get;set;}
    
   }
   public class Objects
   {
       public string  type{get;set;}
       public string name{get;set;}
       public string content{get;set;}
   }

   public class MoveLogger
   {
       public string @context{get;set;}
       public string id{get;set;}
       public string summary{get;set;}
       public string type{get;set;}
       public Actor actor{get;set;}
       public string objects{get;set;}
       public Target target{get;set;}
       public Origin origin{get;set;}
   }

   public class RemoveLogger
   {
       public string @context{get;set;}
       public string id{get;set;}
       public string summary{get;set;}
       public string type{get;set;}
       public Actor actor{get;set;}
       public string objects{get;set;}
       public Target target{get;set;}
   }

   public class UpdateLogger
   {
       public string @context{get;set;}
       public string id{get;set;}
       public string summary{get;set;}
       public string type{get;set;}
       public Actor actor{get;set;}
       public string objects{get;set;}
   }

   public class AddLogger
   {
       public string @context{get;set;}
       public string id{get;set;}
       public string summary{get;set;}
       public string type{get;set;}
       public Actor actor{get;set;}
       public string objects{get;set;}
   }

   
   
}
