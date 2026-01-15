using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.UFP;

namespace CatOs.Core.Interface.UFP
{
    public interface IUfp
    {
        public Task<ReturnResult<object>> ListUfp(Seach seach);
        public Task<ReturnResult<string>> CreaterUfp(NewUfp newUfp);
        public Task<ReturnResult<string>> UpdateUfp(UpdateUfp updateUfp);
        public Task<ReturnResult<string>> DeleteUfp(Guid ufpUuid);

        public Task<ReturnResult<string>> AssignUfpToSku(AssignUfpToSku assignUfpToSku);
        public Task<ReturnResult<string>> UpdateAssingUfpToSku(UpdateAssingUfpToSku updateAssingUfpToSku);
        public Task<ReturnResult<string>> RemoveUfpFromSku(Guid skuUfpUuid);
    }
}
