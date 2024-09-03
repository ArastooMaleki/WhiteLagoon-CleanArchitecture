using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.Models;
namespace WhiteLagoon.Web.Controllers
{/*
    [Route("/Villa")]*/
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VillaController(IUnitOfWork unitOfWork
            , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        [HttpGet]/*
        [ActionName("Index")]*/
        public async Task<IActionResult> IndexAsync()
        {
            var viewModel = _mapper.Map<List<VillaIndexViewModel>>(_unitOfWork.VillaRepository.GetAllAsync().GetAwaiter().GetResult());
            return View("Index", viewModel);
        }
        [HttpDelete]/*
        [Route("/Villa/Delete/{id}")]*/
        public async Task<IActionResult> DeleteAsync(/*[FromRoute]*/int id)
        {
            var ID = id; 
            var villa = await _unitOfWork.VillaRepository.GetAsync(v => v.Id == ID);
            if (villa == null)
            {
                throw new KeyNotFoundException();
            }
            await _unitOfWork.VillaRepository.DeleteAsync(villa);
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]VillaCreateViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException();
            }
            if (viewModel.Name == null)
            {
                throw new NullReferenceException();
            }
            await _unitOfWork.VillaRepository.AddAsync(_mapper.Map<Villa>(viewModel));
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody]VillaUpdateViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException();
            }
            if(viewModel.Id == 0 || viewModel.Name == null || viewModel.Description == null)
            {
                throw new NullReferenceException();
            }
            await _unitOfWork.VillaRepository.UpdateAsync(_mapper.Map<Villa>(viewModel));
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
