using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Kibin.Models;
using System;
namespace Kibin.Services
{
  public class ProjectService: IProjectService
  {
      private readonly IMongoCollection<Project> _user;
      public ProjectService(IMongoDBContext context)
      {
           _user = context.Database().GetCollection<Project>("ProjectCollection3");
          
      }
      public List<Project> Get() =>
           _user.Find(user => true).ToList();
       public Project Get(string id) =>
           _user.Find<Project>(user => user.Id == id).FirstOrDefault();
    //    public List<Project> GetByOwner(string id)=>
    //    _user.Find(user => {
    //        bool flag=false;bool flag2=false;
    //        foreach(var i in user.members)
    //        {Console.WriteLine(i);}
    //        return true;
    //    }).ToList();

    // public List<Project> GetByOwner(string id)=>
    // _user.Find(user=>check(user,id)).ToList();
    // public bool check(Project user,string id)
    // {
    //     bool flag=false;bool flag2=false;
    //        foreach(var i in user.members)
    //        {Console.WriteLine(i);}
    //        return true;
    // }
    public List<Project> GetByOwner(string id){

        
        return _user.Find(user=> true).ToList();
    
    }
       
       public Project Create(Project user)
       {
           _user.InsertOne(user);
           return user;
       }
       public void Update(string id, Project userIn) =>
           _user.ReplaceOne(user => user.Id == id, userIn);
       public void Remove(Project userIn) =>
           _user.DeleteOne(user => user.Id == userIn.Id);
       public void Remove(string id) =>
           _user.DeleteOne(user => user.Id == id);
  }
  public interface IProjectService
  {
      List<Project> Get();
        Project Get(string id);

        Project Create(Project entity);
        void Update(string id,Project project);
        void Remove(string id);
        List<Project> GetByOwner(string id);
  }
}