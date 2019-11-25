using System;
using System.Collections.Generic;
using System.Text;
using Kibin.Models;
using Kibin.Services;
using Xunit;

using Kibin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Kibin.Tests;

namespace Kibin.Tests
{
    public class TasksControllerTest
    {
        TasksController _controller;
        ITaskService _service;
        ILoggerService _logger;

        public TasksControllerTest()
        {
            _service = new TaskServiceTest();
            _controller = new TasksController(_service,_logger);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            //Act
            var okResult = _controller.Get();

            //Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.Get().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Tasks>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        //[Fact]
        //public void Create_ValidData_Return_OkResult()
        //{
        //    //Arrange  
        //    var tasks = new Tasks() { TaskId = "dgfgg", id = 8765434, action = "post", unique_id = "fdgff", project_id = "fgfesf", start_date = 76543446, end_date = 4635757, text = "Testing456", progress = 0.6, duration = 6 };

        //    //Act  
        //    var data = _controller.Create(tasks, "fhhf345678", "2456", "456");

        //    //Assert  
        //    Assert.IsType<CreatedAtRouteResult>(data);
        //}


        //[Fact]
        //public void Create_InvalidObjectPassed_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var nameMissingItem = new Tasks()
        //    {
        //        TaskId = "abcde",
        //        id = 345678345,
        //        action = "post",
        //        unique_id = "fghij",
        //        project_id = "klmno",
        //        start_date = 2345674,
        //        end_date = 123456789,
        //        text = "Testing123",
        //        progress = 0.2,
        //        duration = 3
        //    };


        //    _controller.ModelState.AddModelError("epicId", "Required");

        //    // Act
        //    var badResponse = _controller.Create(nameMissingItem, "fhhf345678", "2456", "456");

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(badResponse);
        //}

       // [Fact]
        //public void Create_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        //{
        //    // Arrange
        //    var testItem = new Tasks()
        //    {
        //        TaskId = "abcde",
        //        id = 345678345,
        //        action = "post",
        //        unique_id = "fghij",
        //        project_id = "klmno",
        //        start_date = 2345674,
        //        end_date = 123456789,
        //        text = "Testing123",
        //        progress = 0.2,
        //        duration = 3
        //    };

        //    // Act
        //    var createdResponse = _controller.Create(testItem, "fhhf345678", "2456", "456");// as CreatedAtRouteResult;
        //    var item = createdResponse.Value as Tasks;

        //    // Assert
        //    Assert.IsType<Tasks>(item);
        //    Assert.Equal("abc", item.text);
        //}
    }
}
 
