using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.Stock;
using CatOs.Core.Interface.Stock;
using Microsoft.AspNetCore.Mvc;

namespace CatOs.ApiHost.Controllers.Stock
{
    [Route("api/vi/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStock _stock;
        public StockController(IStock stock)
        {
            _stock = stock;
        }

        [HttpGet]
        public async Task<IActionResult> ListStock([FromQuery] Seach seach)
        {
            var result = await _stock.ListStock(seach);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> AddItemStock([FromBody] AddItemStock addItemStock)
        {
            var result = await _stock.AddItemStock(addItemStock);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateItemStock([FromBody] UpdateItemStock updateItemStock)
        {
            var result = await _stock.UpdateItemStock(updateItemStock);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("{uuid}")]
        public async Task<IActionResult> DeleteItemStock([FromRoute] Guid uuid)
        {
            var result = await _stock.DeleteItemStock(uuid);
            return StatusCode(result.StatusCode, result);
        }
    }
}
