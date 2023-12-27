using FPT_Exchange_Data.DTO.Internal;
using FPT_Exchange_Data.DTO.Request.Get;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.View;
using FPT_Exchange_Utility.Helpers;
using Microsoft.Extensions.Options;

namespace FPT_Exchange_Service.Payment
{
    public class MomoPaymentService : IMomoPaymentService
    {
        private readonly MomoConfiguration momoConfig;

        public MomoPaymentService(IOptions<MomoConfiguration> momoConfigOptions)
        {
            this.momoConfig = momoConfigOptions.Value;
        }


        public async Task<MomoPaymentViewModel> Create(MomoPaymentRequest info, string userId)
        {
            string generateId = Guid.NewGuid().ToString();
            MomoOneTimePaymentRequest momoPaymentRequest = new MomoOneTimePaymentRequest
            {

                PartnerCode = momoConfig.PartnerCode,
                RequestId = Guid.NewGuid().ToString(),
                Amount = info.Amount,
                OrderId = Guid.NewGuid().ToString(),
                OrderInfo = info.Content,
                IpnUrl = momoConfig.IpnUrl,//Response result,
                RedirectUrl = momoConfig.ReturnUrl + $"/{userId}",
                RequestType = "captureWallet",
                ExtraData = string.Empty,
            };

            momoPaymentRequest.MakeSignature(momoConfig.AccessKey, momoConfig.SecretKey);
            (bool createMomoLinkResult, string? createMessage) = momoPaymentRequest.GetLink(momoConfig.PaymentUrl);
            if (createMomoLinkResult)
            {

                return new MomoPaymentViewModel
                {
                    PaymentId = generateId,
                    Url = createMessage,

                };
            }
            else
            {
                return null;
            }

        }

        public async Task<MomoPaymentResultViewModel> HandleResult(MomoPaymentResultRequest request, string userId)
        {
            var isValidSignature = request.IsValidSignature(momoConfig.AccessKey, momoConfig.SecretKey);

            if (isValidSignature)
            {
                if (request.ResultCode == 0)//susscess
                {
                    var (currentScore, check) = await ApiHelper.UpdateWallet(userId, (int)request.Amount);

                    if (check)
                    {
                        return new MomoPaymentResultViewModel
                        {
                            PaymentId = request.OrderId,
                            AddedScore = (int)request.Amount,
                            CurrentScore = (int)currentScore,
                        };
                    }
                    // handle transaction again if fail add money
                    return null;
                }
            }
            return null;
        }
    }
}
