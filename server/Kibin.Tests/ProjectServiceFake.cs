// // using Kibin.Data;
// using Kibin.Models;
// using Kibin.Services;
// using MongoDB.Driver;
// using System;
// using System.Collections.Generic;
// using System.Text;

// namespace Kibin.Tests
// {
//     class ProjectServiceFake : IProjectService
//     {
//         //private readonly IMongoCollection<Project> _entity;
//         //private readonly string _table = "Project";
//         private List<Project> _project;
//         public ProjectServiceFake()
//         {
//             _project = new List<Project>()
//             {
//                 new Project(){ Id = "abcf", projectName = "abc", projectDescription = "abc",
//                     projectType = "kanban", startTime = new DateTime(2019, 10, 1),
//                     endTime = new DateTime(2019, 10, 22),
//                     epicDetails = new EpicDetails[1] { new EpicDetails() { epicId = "111", epicName = "epic name" } },
//                     owner = "me", members = new string[1] { "member1" } },
//                 new Project(){ Id = "abcv", projectName = "abc", projectDescription = "abc",
//                     projectType = "kanban", startTime = new DateTime(2019, 10, 1),
//                     endTime = new DateTime(2019, 10, 22),
//                     epicDetails = new EpicDetails[1] { new EpicDetails() { epicId = "111", epicName = "epic name" } },
//                     owner = "me", members = new string[1] { "member1" } }
//             };



//         }

//         public List<Project> Get()
//         {
//             return _project;
//         }
//         //_entity.Find(entity => true).Sort("{Score: -1}").ToList();


//         public Project Get(string id)
//         {
//             return _project[0];
//         }
//         public Project GetUserId(string id)
//         {
//             return _project[0];

//         }
        

//         // public Project GetByUser(string id)
//         // {
//         //     var entity = new Project() { Id = "sdfsdfkj", UserId = id, UserName = "Sager sdkfjsd", Score = 100 };
//         //     return entity;
//         // }
//         //_entity.Find<Project>(entity => entity.UserId == id).FirstOrDefault();

//         public Project Create(Project entity)
//         {
//             //_entity.InsertOne(entity);
//             //return entity;
//             _project.Add(entity);
//             return entity;
//         }


//         public void Update(string id,Project k)
//         {

//         }
//         public void Remove(string id)
//         {

//         }
//         // _entity.ReplaceOne(entity => entity.Id == id, entityIn);
//         // Builders<Project>.Update.Set(lb => lb.Score += score);
//         //_entity.UpdateOne(
//         //    entity => entity.UserId == entityIn.id,
//         //    Builders<Project>.Update.Inc(lb => lb.Score, entityIn.score)
//         //);




//     }
// }

