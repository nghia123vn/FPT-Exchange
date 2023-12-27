using FPT_Exchange_Data.DTO.Request.Get;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.View;

namespace FPT_Exchange_Service.Payment
{
    public interface IMomoPaymentService
    {
        public Task<MomoPaymentViewModel> Create(MomoPaymentRequest info, string userId);

        public Task<MomoPaymentResultViewModel> HandleResult(MomoPaymentResultRequest result, string id);
    }
}
