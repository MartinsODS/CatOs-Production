using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.UFP;
using CatOs.Core.Interface.UFP;
using Microsoft.AspNetCore.Mvc;

namespace CatOs.ApiHost.Controllers.UFP
{
    [Route("api/[controller]")]
    [ApiController]
    public class UfpController : ControllerBase
    {
        private readonly IUfp _ufp;
        public UfpController(IUfp ufp)
        {
            _ufp = ufp;
        }
        [HttpGet]
        public async Task<IActionResult> ListUfp([FromQuery] Seach seach)
        {
            var result = await _ufp.ListUfp(seach);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> CreaterUfp([FromBody] NewUfp newUfp)
        {
            var result = await _ufp.CreaterUfp(newUfp);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUfp([FromBody] UpdateUfp updateUfp)
        {
            var result = await _ufp.UpdateUfp(updateUfp);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("{uuid}")]
        public async Task<IActionResult> DeleteUfp(Guid uuid)
        {
            var result = await _ufp.DeleteUfp(uuid);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("assign-skus")]
        public async Task<IActionResult> AssignSkus([FromBody] AssignUfpToSku assignSkus)
        {
            var result = await _ufp.AssignUfpToSku(assignSkus);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("assign-skus")]
        public async Task<IActionResult> UpdateAssignSkus([FromBody] UpdateAssingUfpToSku updateAssignSkus)
        {
            var result = await _ufp.UpdateAssingUfpToSku(updateAssignSkus);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("assign-skus/{ufpSkuUuid}")]
        public async Task<IActionResult> RemoveAssignSkus(Guid ufpSkuUuid)
        {
            var result = await _ufp.RemoveUfpFromSku(ufpSkuUuid);
            return StatusCode(result.StatusCode, result);
        }
    }
}
