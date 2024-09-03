using WhiteLagoon.Web.Controllers;
using Moq;
using AutoFixture;
using WhiteLagoon.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Web.Models;
using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interfaces;
using AutoMapper;
using WhiteLagoon.Web.AutoMapper;
namespace WhiteLagoon.Tests
{
    public class VillaControllerTests
    {
        private readonly Fixture _fixture;
        private readonly IEnumerable<Villa> _returnedVillas;
        private readonly Mock<IUnitOfWork> _villaUnitOfWorkMoq;
        private readonly VillaController controller;
        private readonly IMapper _mapper;
        public VillaControllerTests()
        {
            this._fixture = new Fixture() { RepeatCount = 10 };
            this._returnedVillas = _fixture.CreateMany<Villa>();
            this._villaUnitOfWorkMoq = new Mock<IUnitOfWork>();
            this._mapper = new MapperConfiguration(opt =>
            {
                opt.AddProfile(typeof(AutoMapperProfile));
            }).CreateMapper();
        }

        #region Index

        [Fact]
        public async Task Index_ShouldReturnView()
        {
            //Assert
            _villaUnitOfWorkMoq.Setup(repo => repo.VillaRepository.GetAllAsync(It.IsAny<Expression<Func<Villa, bool>>>(), It.IsAny<bool>())).ReturnsAsync(_returnedVillas);
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            //Action
            var result = await controller.IndexAsync();

            //Assert 
            var viewResult = Assert.IsType<ViewResult>(result);

        }
        [Fact]
        public async Task Index_ShouldReturnVillaIndexViewModelInView()
        {
            //Assert
            _villaUnitOfWorkMoq.Setup(repo => repo.VillaRepository.GetAllAsync(It.IsAny<Expression<Func<Villa, bool>>>(), It.IsAny<bool>())).ReturnsAsync(_returnedVillas);
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);

            //Action
            var result = await controller.IndexAsync();

            //Assert 
            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeOfType<List<VillaIndexViewModel>>();

        }
        [Fact]
        public async Task Index_VillasShouldExactlyBeLikeEntredOne()
        {
            //Assert
            _villaUnitOfWorkMoq.Setup(repo => repo.VillaRepository.GetAllAsync(It.IsAny<Expression<Func<Villa, bool>>>(), It.IsAny<bool>())).ReturnsAsync(_returnedVillas);
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);

            //Action
            var result = await controller.IndexAsync();

            //Assert 
            var viewResult = result as ViewResult;
            var viewModel = viewResult.Model as List<VillaIndexViewModel>;
            _mapper.Map<List<VillaIndexViewModel>>(_returnedVillas).Should().BeEquivalentTo(viewModel);
        }
        #endregion
        #region Delete
        [Fact]
        public void Delete_InputNotIdNumber_ShouldThrowArgumentNullException()
        {
            //Arrange
            _villaUnitOfWorkMoq.Setup(u => u.VillaRepository.GetAsync(It.IsAny<Expression<Func<Villa, bool>>>(), It.IsAny<bool>())).ReturnsAsync((Villa)null);
            var deleteController = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await deleteController.DeleteAsync(1));
        }
        [Fact]
        public async Task Delete_InputId_ReturnRedirectToActionResult()
        {
            _villaUnitOfWorkMoq.Setup(u => u.VillaRepository.GetAsync(It.IsAny<Expression<Func<Villa, bool>>>(), It.IsAny<bool>())).ReturnsAsync(_fixture.Create<Villa>());
            var deleteController = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            //Assert
            var result = await deleteController.DeleteAsync(1);
            Assert.IsType<RedirectToActionResult>(result);
        }
        #endregion
        #region Add
        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_IfInputIsNull()
        {
            //Arrange 
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            Assert.ThrowsAsync<ArgumentNullException>(async () => await controller.CreateAsync(null));
        }
        [Fact]
        public async Task Create_ShouldThrowNullRefrenceException_IfNameOrDescreptionIsNull()
        {
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            Assert.ThrowsAsync<NullReferenceException>(async () => await controller.CreateAsync(new VillaCreateViewModel()));
        }
        [Fact]
        public async Task Create_ShouldReturnRedirectToAction()
        {
            _villaUnitOfWorkMoq.Setup(v => v.VillaRepository.AddAsync(It.IsAny<Villa>()));
            _villaUnitOfWorkMoq.Setup(v => v.SaveAsync());
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            var result = await controller.CreateAsync(_fixture.Create<VillaCreateViewModel>());
            var x = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", x.ActionName);
        }
        [Fact]
        public async Task Create_ShouldReturnRedirectToActionIndex()
        {
            _villaUnitOfWorkMoq.Setup(v => v.VillaRepository.AddAsync(It.IsAny<Villa>()));
            _villaUnitOfWorkMoq.Setup(v => v.SaveAsync());
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            var result = await controller.CreateAsync(_fixture.Create<VillaCreateViewModel>());
            var x = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", x.ActionName);
        }
        #endregion
        #region Update
        [Fact]
        public async Task Update_ShouldThrowArgumentNullException_IfInputIsNull()
        {
            //Arrange 
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await controller.UpdateAsync(null));
        }
        [Fact]
        public async Task Update_ShouldThrowNullRefrenceException_IfNameOrDescreptionIsNull()
        {
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            await Assert.ThrowsAsync<NullReferenceException>(async () => await controller.UpdateAsync(new VillaUpdateViewModel()));
        }
        [Fact]
        public async Task Update_ShouldReturnRedirectToAction()
        {
            _villaUnitOfWorkMoq.Setup(v => v.VillaRepository.AddAsync(It.IsAny<Villa>()));
            _villaUnitOfWorkMoq.Setup(v => v.SaveAsync());
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            var result = await controller.UpdateAsync(_fixture.Create<VillaUpdateViewModel>());
            var x = Assert.IsType<RedirectToActionResult>(result);
        }
        [Fact]
        public async Task Update_ShouldReturnRedirectToActionIndex()
        {
            _villaUnitOfWorkMoq.Setup(v => v.VillaRepository.AddAsync(It.IsAny<Villa>()));
            _villaUnitOfWorkMoq.Setup(v => v.SaveAsync());
            var controller = new VillaController(_villaUnitOfWorkMoq.Object, _mapper);
            var result = await controller.UpdateAsync(_fixture.Create<VillaUpdateViewModel>());
            var x = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", x.ActionName);
        }
        #endregion
    }
}
