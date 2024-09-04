using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    [Area("VillasController")]
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await _unitOfWork.VillaNumberRepository.GetAllAsync();
            return Ok(viewModel);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var VillaNumber = await _unitOfWork.VillaNumberRepository.GetAsync(vn => vn.Villa_Number == id);
            if (VillaNumber == null)
            {
                return NotFound();
            }
            return Ok(VillaNumber);

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VillaNumber updatedVillaNumber)
        {
            if(updatedVillaNumber == null)
            {
                return BadRequest();
            }
            if (updatedVillaNumber.Villa_Number == 0)
            {
                return BadRequest();
            }
            VillaNumber oldVillaNumber = await _unitOfWork.VillaNumberRepository.GetAsync(
                vn=>vn.Villa_Number == updatedVillaNumber.Villa_Number);
            if (oldVillaNumber == null)
            {
                return NotFound();
            }
            oldVillaNumber.VillaId = updatedVillaNumber.VillaId;
            oldVillaNumber.SpecialDetails = updatedVillaNumber.SpecialDetails;
            await _unitOfWork.VillaNumberRepository.UpdateAsync(oldVillaNumber);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VillaNumber villaNumber)
        {
            if (villaNumber == null)
            {
                return BadRequest();
            }
            if (villaNumber.Villa_Number == 0)
            {
                return BadRequest();
            }
            await _unitOfWork.VillaNumberRepository.AddAsync(villaNumber);
            await _unitOfWork.SaveAsync();
            return Ok();

        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id )
        {
            if (id == 0)
            {
                return BadRequest();
            }
            VillaNumber oldVillaNumber = await _unitOfWork.VillaNumberRepository.GetAsync(
                vn => vn.Villa_Number == id);
            if (oldVillaNumber == null)
            {
                return NotFound();
            }
            await _unitOfWork.VillaNumberRepository.DeleteAsync(oldVillaNumber);
            await _unitOfWork.SaveAsync();  
            return Ok();
        }

    }
}
