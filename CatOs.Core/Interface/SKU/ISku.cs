using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.SKU;

namespace CatOs.Core.Interface.SKU
{
    public interface ISku
    {
        public Task<ReturnResult<object>> ListSkus(Seach seach);
        public Task<ReturnResult<string>> AddSku(AddSku addSku);
        public Task<ReturnResult<string>> UpdateSku(UpdateSku updateSku);
        public Task<ReturnResult<string>> DeleteSku(Guid skuUuid);
        public Task<ReturnResult<string>> AddComponents(AddComponent addComponent);
        public Task<ReturnResult<string>> UpdateComponents(UpdateComponent updateComponent);
        public Task<ReturnResult<string>> RemoveComponents(Guid skuComponentUuid);

    }
}
