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
//     public class ProjectControllerTest
//     {
//         ProjectController _controller;
//         IProjectService _service;

//         public ProjectControllerTest()
//         {
//             _service = new ProjectServiceFake();
//             _controller = new ProjectController(_service);
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
//             var items = Assert.IsType<List<Project>>(okResult.Value);
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
//             var projectobj =  new Project() { Id = "abcf", projectName = "abc", projectDescription = "abc", projectType = "kanban", startTime = new DateTime(2019, 10, 1), endTime = new DateTime(2019, 10, 22), epicDetails = new EpicDetails[1] { new EpicDetails() { epicId = "111", epicName = "epic name" } }, owner = "me", members = new string[1] { "member1" } };

//             //Act  
//                 var data = _controller.Create(projectobj);

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
//             new Project()
//             { Id = "abcf",
//                 projectName = "abc",
//                 projectDescription = "abc",
//                 projectType = "kanban",
//                 startTime = new DateTime(2019, 10, 1),
//                 endTime = new DateTime(2019, 10, 22),
//                 epicDetails = new EpicDetails[1] { new EpicDetails() { epicId = "111", epicName = "epic name" } },
                
//                 members = new string[1] { "member1" }
//             };

//             _controller.ModelState.AddModelError("owner", "Required");

//             // Act
//             var badResponse = _controller.Create(nameMissingItem);

//             // Assert
//             Assert.IsType<BadRequestObjectResult>(badResponse);
//         }

//         [Fact]
//         public void Create_ValidObjectPassed_ReturnedResponseHasCreatedItem()
//         {
//             // Arrange
//             var testItem = new Project()
//             {
//                 Id = "abcf",
//                 projectName = "abc",
//                 projectDescription = "abc",
//                 projectType = "kanban",
//                 startTime = new DateTime(2019, 10, 1),
//                 endTime = new DateTime(2019, 10, 22),
//                 epicDetails = new EpicDetails[1] { new EpicDetails() { epicId = "111", epicName = "epic name" } },
//                 owner="me",
//                 members = new string[1] { "member1" }
//             };

//             // Act
//             var createdResponse = _controller.Create(testItem) as CreatedAtRouteResult;
//             var item = createdResponse.Value as Project;

//             // Assert
//             Assert.IsType<Project>(item);
//             Assert.Equal("me", item.owner);
//         }
//     }

   
// }
