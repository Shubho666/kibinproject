// using Kibin.Controllers;
// using Kibin.Models;
// using Kibin.Services;
// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Collections.Generic;
// using System.Text;
// using Xunit;
// using Kibin.Tests;

// namespace Kibin.Tests
// {
//     public class KanbanUSControllerTest
//     {
//         KanbanUSController _controller;
//         IKanbanUSService _service;

//         public KanbanUSControllerTest()
//         {
//             _service = new KanbanUserStoryServiceFake();
//             _controller = new KanbanUSController(_service);
//         }

//         [Fact]
//         public void Get_WhenCalled_ReturnsOkResult()
//         {
//             //Act
//             var okResult = _controller.Get();

//             //Assert
//             Assert.IsType<OkObjectResult>(okResult.Result);
//         }

//         [Fact]
//         public void Get_WhenCalled_ReturnsAllItems()
//         {
//             // Act
//             var okResult = _controller.Get().Result as OkObjectResult;

//             // Assert
//             var items = Assert.IsType<List<KanbanUserStory>>(okResult.Value);
//             Assert.Equal(2, items.Count);
//         }

//         //[Fact]
//         //public void Get_WhenCalled_ReturnsBadRequest()
//         //{
//         //    //Act
//         //    var data = _controller.Get();
//         //    data = null;

//         //    if(data != null)
//         //    {
//         //        //Assert
//         //        Assert.IsType<BadRequestResult>(data);
//         //    }
//         //}

//         [Fact]
//         public void Create_ValidData_Return_OkResult()
//         {
//             //Arrange  
//             var kanbanus =  new KanbanUserStory() { Id = "abcf", description = "abc", shortName = "abc", projectId = "abc", userId = "a", status = "a", acceptanceCriteria = new string[4] { "Sun", "Mon", "Tue", "Wed" }, tasks = new Task[1] { new Task() { taskId = "123", taskStatus = "a", assigneeId = new string[1] { "233" }, taskDescription = "a" } }, epicId = "chu" };

//             //Act  
//             var data = _controller.Create(kanbanus);

//             //Assert  
//             Assert.IsType<CreatedAtRouteResult>(data);
//         }

//         //[Fact]
//         //public void Create_InvalidData_Return_BadRequest()
//         //{
//         //    //Arrange  
//         //    var leaderboard = new Leaderboard() { Id = "ijkl", UserId = "dipanjan.sarkar", UserName = "Dipanjan Sarkar"};

//         //    //Act              
//         //    var data = _controller.Create(leaderboard);

//         //    //Assert  
//         //    Assert.IsType<BadRequestResult>(data);
//         //}

//         [Fact]
//         public void Create_InvalidObjectPassed_ReturnsBadRequest()
//         {
//             // Arrange
//             var nameMissingItem =
//             new KanbanUserStory()
//             {
//                 Id = "abcd",
//                 description = "abc",
//                 shortName = "abc",
//                 projectId = "abc",
//                 userId = "a",
//                 status = "a",
//                 acceptanceCriteria = new string[4] { "Sun", "Mon", "Tue", "Wed" },
//                 tasks = new Task[1] { new Task() { taskId = "123", taskStatus = "a",
//                     assigneeId = new string[1] { "233" }, taskDescription = "a" } },
                
//             };
//             _controller.ModelState.AddModelError("epicId", "Required");

//             // Act
//             var badResponse = _controller.Create(nameMissingItem);

//             // Assert
//             Assert.IsType<BadRequestObjectResult>(badResponse);
//         }

//         [Fact]
//         public void Create_ValidObjectPassed_ReturnedResponseHasCreatedItem()
//         {
//             // Arrange
//             var testItem = new KanbanUserStory()
//             {
//                 Id = "abcd",
//                 description = "abc",
//                 shortName = "abc",
//                 projectId = "abc",
//                 userId = "a",
//                 status = "a",
//                 acceptanceCriteria = new string[4] { "Sun", "Mon", "Tue", "Wed" },
//                 tasks = new Task[1] { new Task() { taskId = "123", taskStatus = "a",
//                     assigneeId = new string[1] { "233" }, taskDescription = "a" } },
//                 epicId="chu"

//             };

//             // Act
//             var createdResponse = _controller.Create(testItem) as CreatedAtRouteResult;
//             var item = createdResponse.Value as KanbanUserStory;

//             // Assert
//             Assert.IsType<KanbanUserStory>(item);
//             Assert.Equal("abc", item.description);
//         }
//     }

   
// }
