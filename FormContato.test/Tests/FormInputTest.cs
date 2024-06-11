//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FormContato;
//using FluentAssertions;
//using FormContato.Context;
//using FormContato.Controllers;
//using Moq.AutoMock;
//using Moq;
//using FormContato.DTOs;
//using Confluent.Kafka;
//using FormContato.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore;

//namespace FormContato.test.Tests;
//public class FormInputTest
//{

//    private readonly Mock<FCDbContext> _mockContext;
//    private readonly HomeController _homeController;

//    public FormInputTest()
//    {
//        var options = new DbContextOptionsBuilder<FCDbContext>()
//                          .UseInMemoryDatabase(databaseName: "TestDatabase")
//                          .Options;

//        _mockContext = new Mock<FCDbContext>(options);
//        _homeController = new HomeController(_mockContext.Object);

//    }

//    // deve: receber os dados definidos no DTO e salvar no DB 

//    [Fact]
//    public async Task SaveMessage_InputMatchesDTO_ReturnsSucessView()
//    {
//        // arrange
//        var contato = new ContactDTO
//        {
//            Nome = "Teste",
//            Email = "teste@teste.com",
//            Mensagem = "Mensagem de teste"
//        };

//        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

//        // act
//        var result = await _homeController.SaveMessage(contato);

//        // assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.Equal("Success", viewResult.ViewName);

//        _mockContext.Verify(c => c.Add(It.IsAny<ContactViewModel>()), Times.Once);
//        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//    }

//}


////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using FormContato;
////using FluentAssertions;
////using FormContato.Context;
////using FormContato.Controllers;
////using Moq;
////using FormContato.DTOs;
////using Confluent.Kafka;
////using FormContato.Models;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.Extensions.Logging;

////namespace FormContato.test.Tests;
////public class FormInputTest
////{
////    private readonly Mock<ILogger<HomeController>> _mockLogger;
////    private readonly Mock<FCDbContext> _mockContext;
////    private readonly HomeController _homeController;

////    public FormInputTest()
////    {
////        _mockLogger = new Mock<ILogger<HomeController>>();
////        _mockContext = new Mock<FCDbContext>();
////        _homeController = new HomeController(_mockLogger.Object, _mockContext.Object);
////    }

////    // deve: receber os dados definidos no DTO e salvar no DB 

////    [Fact]
////    public async Task SaveMessage_InputMatchesDTO_ReturnsSucessView()
////    {
////        // arrange
////        var contato = new ContactDTO
////        {
////            Nome = "Teste",
////            Email = "teste@teste.com",
////            Mensagem = "Mensagem de teste"
////        };

////        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

////        // act
////        var result = await _homeController.SaveMessage(contato);

////        // assert
////        var viewResult = Assert.IsType<ViewResult>(result);
////        Assert.Equal("Success", viewResult.ViewName);

////        _mockContext.Verify(c => c.Add(It.IsAny<ContactViewModel>()), Times.Once);
////        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
////    }
////}
