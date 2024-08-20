using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.Models;
namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var viewModel = new VillaIndexViewModel() { villas = await _unitOfWork.VillaRepository.GetAllAsync() };
            return View(viewModel);
        }

        public async Task<IActionResult> DeleteAsync(int ID)
        {
            var villa = await _unitOfWork.VillaRepository.GetAsync(v => v.Id == ID);
            if (villa == null)
            {
                throw new KeyNotFoundException();
            }
            await _unitOfWork.VillaRepository.DeleteAsync(villa);
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateAsync(VillaCreateViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException();
            }
            if (viewModel.villa == null)
            {
                throw new NullReferenceException();
            }
            await _unitOfWork.VillaRepository.AddAsync(viewModel.villa);
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateAsync(VillaUpdateViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException();
            }
            if(viewModel.villa.Id == 0 || viewModel.villa.Name == null || viewModel.villa.Description == null)
            {
                throw new NullReferenceException();
            }
            await _unitOfWork.VillaRepository.UpdateAsync(viewModel.villa);
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
