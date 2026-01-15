using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.SKU;
using CatOs.Core.Interface.SKU;
using Microsoft.AspNetCore.Mvc;

namespace CatOs.ApiHost.Controllers.SKU
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkuController : ControllerBase
    {
        private readonly ISku _sku;
        public SkuController(ISku sku)
        {
            _sku = sku;
        }
        [HttpGet]
        public async Task<ActionResult> ListSku([FromQuery] Seach seach)
        {
            var result = await _sku.ListSkus(seach);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        public async Task<ActionResult> AddSku([FromBody] AddSku addSku)
        {
            var result = await _sku.AddSku(addSku);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateSku([FromBody] UpdateSku updateSku)
        {
            var result = await _sku.UpdateSku(updateSku);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("{skuUuid}")]
        public async Task<ActionResult> DeleteSku([FromRoute] Guid skuUuid)
        {
            var result = await _sku.DeleteSku(skuUuid);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("components")]
        public async Task<ActionResult> AddComponents([FromBody] AddComponent addComponent)
        {
            var result = await _sku.AddComponents(addComponent);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("components")]
        public async Task<ActionResult> UpdateComponents([FromBody] UpdateComponent updateComponent)
        {
            var result = await _sku.UpdateComponents(updateComponent);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("components/{skuComponentUuid}")]
        public async Task<ActionResult> RemoveComponents([FromRoute] Guid skuComponentUuid)
        {
            var result = await _sku.RemoveComponents(skuComponentUuid);
            return StatusCode(result.StatusCode, result);
        }
    }
}
