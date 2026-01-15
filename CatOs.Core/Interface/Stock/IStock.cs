using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.Stock;

namespace CatOs.Core.Interface.Stock
{
    public interface IStock
    {
        public Task<ReturnResult<object>> ListStock(Seach seach);
        public Task<ReturnResult<string>> AddItemStock(AddItemStock addItemStock);
        public Task<ReturnResult<string>> UpdateItemStock(UpdateItemStock updateItemStock);
        public Task<ReturnResult<string>> DeleteItemStock(Guid uuid);
    }
}
