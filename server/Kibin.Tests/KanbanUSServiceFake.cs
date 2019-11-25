// // using Kibin.Data;
// using Kibin.Models;
// using Kibin.Services;
// using MongoDB.Driver;
// using System;
// using System.Collections.Generic;
// using System.Text;

// namespace Kibin.Tests
// {
//     class KanbanUserStoryServiceFake : IKanbanUSService
//     {
//         //private readonly IMongoCollection<KanbanUserStory> _entity;
//         //private readonly string _table = "KanbanUserStory";
//         private List<KanbanUserStory> _kanbanUserStory;
//         public KanbanUserStoryServiceFake()
//         {
//             _kanbanUserStory = new List<KanbanUserStory>()
//             {
//                 new KanbanUserStory(){Id = "abcd", description = "abc",shortName="abc",projectId="abc",userId="a",status="a",acceptanceCriteria=new string[4] {"Sun", "Mon", "Tue", "Wed"},tasks=new Task[1] { new Task() { taskId = "123", taskStatus = "a", assigneeId = new string[1] { "233" }, taskDescription = "a" } },epicId="chu" },
//                 new KanbanUserStory(){Id = "abce", description = "abhhbhbc",shortName="abhjc",projectId="abc",userId="a",status="a",acceptanceCriteria=new string[4] {"Sun", "Mon", "Tue", "Wed"},tasks=new Task[1] { new Task() { taskId = "123", taskStatus = "a", assigneeId = new string[1] { "233" }, taskDescription = "a" } },epicId="chu" }
//             };
//         }

//         public List<KanbanUserStory> Get()
//         {
//             return _kanbanUserStory;
//         }
//         //_entity.Find(entity => true).Sort("{Score: -1}").ToList();


//         public KanbanUserStory Get(string id)
//         {
//             return _kanbanUserStory[0];
//         }
//         public KanbanUserStory GetUserId(string id)
//         {
//             return _kanbanUserStory[0];

//         }
//         public List<KanbanUserStory> GetLastUserId(string id)
//         {
//             return _kanbanUserStory;

//         }

//         // public KanbanUserStory GetByUser(string id)
//         // {
//         //     var entity = new KanbanUserStory() { Id = "sdfsdfkj", UserId = id, UserName = "Sager sdkfjsd", Score = 100 };
//         //     return entity;
//         // }
//         //_entity.Find<KanbanUserStory>(entity => entity.UserId == id).FirstOrDefault();

//         public KanbanUserStory Create(KanbanUserStory entity)
//         {
//             //_entity.InsertOne(entity);
//             //return entity;
//             _kanbanUserStory.Add(entity);
//             return entity;
//         }


//         public void Update(string id,KanbanUserStory k)
//         {

//         }
//         public void Remove(string id)
//         {

//         }
//         // _entity.ReplaceOne(entity => entity.Id == id, entityIn);
//         // Builders<KanbanUserStory>.Update.Set(lb => lb.Score += score);
//         //_entity.UpdateOne(
//         //    entity => entity.UserId == entityIn.id,
//         //    Builders<KanbanUserStory>.Update.Inc(lb => lb.Score, entityIn.score)
//         //);




//     }
// }

