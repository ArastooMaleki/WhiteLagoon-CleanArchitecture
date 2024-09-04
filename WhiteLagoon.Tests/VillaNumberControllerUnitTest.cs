
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.Controllers;

namespace WhiteLagoon.Tests
{
    public class VillaNumberControllerUnitTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IUnitOfWork> _UnitOfWorkMock;

        public VillaNumberControllerUnitTest()
        {
            _fixture = new Fixture() { RepeatCount = 5 };
            _UnitOfWorkMock = new Mock<IUnitOfWork>();
        }
        [Fact]
        public async Task Index_ShouldReturnOk()
        {
            //Arrange
            _UnitOfWorkMock
                .Setup(u => u.VillaNumberRepository.GetAllAsync(It.IsAny<Expression<Func<VillaNumber, bool>>>()
                , It.IsAny<bool>()))
                .ReturnsAsync(new List<VillaNumber>());
            var controller = new VillaNumberController(_UnitOfWorkMock.Object);
            //Act 
            var result = await controller.Index();
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task Index_ShouldReturnVillNumberList()
        {
            //Arrange
            _UnitOfWorkMock
                .Setup(u => u.VillaNumberRepository.GetAllAsync(It.IsAny<Expression<Func<VillaNumber, bool>>>()
                , It.IsAny<bool>()))
                .ReturnsAsync(_fixture.CreateMany<VillaNumber>().ToList());
            var controller = new VillaNumberController(_UnitOfWorkMock.Object);
            //Act 
            var result = await controller.Index();
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<VillaNumber>>(okResult.Value);
            Assert.Equal(5, returnValue.Count);
        }
        [Fact]
        public async Task Index_InputId_ShouldReturnNotFoundIfThatIdDoesntExist()
        {
            //Arrange
            _UnitOfWorkMock
                .Setup(u => u.VillaNumberRepository.GetAsync(It.IsAny<Expression<Func<VillaNumber, bool>>>()
                , It.IsAny<bool>()))
                .ReturnsAsync((VillaNumber)null);
            var controller = new VillaNumberController(_UnitOfWorkMock.Object);
            //Act 
            var result = await controller.Index(-1);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Index_InputId_ShouldReturnOkIfThatIdExist()
        {
            //Arrange
            var villaNumber = _fixture.Create<VillaNumber>();
            _UnitOfWorkMock
                .Setup(u => u.VillaNumberRepository.GetAsync(It.IsAny<Expression<Func<VillaNumber, bool>>>()
                , It.IsAny<bool>()))
                .ReturnsAsync(villaNumber);
            var controller = new VillaNumberController(_UnitOfWorkMock.Object);
            //Act 
            var result = await controller.Index(1);
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<VillaNumber>(okResult.Value);
            Assert.Equal(villaNumber, returnValue);
        }
        [Fact]
        public async Task Update_InputNull_ShouldReturnBadRequest()
        {
            //Arrange
            _UnitOfWorkMock.Setup(u => u.VillaNumberRepository.UpdateAsync(It.IsAny<VillaNumber>()));
            _UnitOfWorkMock.Setup(u => u.SaveAsync());
            var controller = new VillaNumberController(_UnitOfWorkMock.Object);
            //Act 
            var result = await controller.Update(null);
            //Assert
            var okResult = Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task Update_IfVillaNumberDoesntExist_ShouldReturnNotFound()
        {
            //Arrange
            _UnitOfWorkMock.Setup(u => u.VillaNumberRepository.UpdateAsync(It.IsAny<VillaNumber>()));
            _UnitOfWorkMock.Setup(u => u.SaveAsync());
            _UnitOfWorkMock
                .Setup(u => u.VillaNumberRepository.GetAsync(It.IsAny<Expression<Func<VillaNumber, bool>>>()
                , It.IsAny<bool>()))
                .ReturnsAsync((VillaNumber)null);
            var controller = new VillaNumberController(_UnitOfWorkMock.Object);
            //Act 
            var result = await controller.Update(_fixture.Create<VillaNumber>());
            //Assert
            var okResult = Assert.IsType<NotFoundResult>(result);
            
        }
        [Fact]
        public async Task Update_IfEveryThingIsOk_ShouldReturnOk()
        {
            //Arrange
            _UnitOfWorkMock.Setup(u => u.VillaNumberRepository.UpdateAsync(It.IsAny<VillaNumber>()));
            _UnitOfWorkMock.Setup(u => u.SaveAsync());
            _UnitOfWorkMock
                .Setup(u => u.VillaNumberRepository.GetAsync(It.IsAny<Expression<Func<VillaNumber, bool>>>()
                , It.IsAny<bool>()))
                .ReturnsAsync(_fixture.Create<VillaNumber>());
            var controller = new VillaNumberController(_UnitOfWorkMock.Object);
            //Act 
            var result = await controller.Update(_fixture.Create<VillaNumber>());
            //Assert
            var okResult = Assert.IsType<OkResult>(result);
            
        }
        [Fact]
        public async Task Update_InputIdIsNull_ShouldReturnBadRequest()
        {
            //Arrange
            _UnitOfWorkMock.Setup(u => u.VillaNumberRepository.UpdateAsync(It.IsAny<VillaNumber>()));
            _UnitOfWorkMock.Setup(u => u.SaveAsync());
            _UnitOfWorkMock.Setup(u => u.VillaNumberRepository.UpdateAsync(It.IsAny<VillaNumber>()));
            _UnitOfWorkMock.Setup(u => u.SaveAsync());
            var controller = new VillaNumberController(_UnitOfWorkMock.Object);
            var newVillaNumber = new VillaNumber() { Villa_Number = 0 };
            //Act 
            var result = await controller.Update(newVillaNumber);
            //Assert
            var okResult = Assert.IsType<BadRequestResult>(result);
        }
    }
}
    
