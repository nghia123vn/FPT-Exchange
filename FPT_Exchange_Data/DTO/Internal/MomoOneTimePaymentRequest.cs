using FPT_Exchange_Utility.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace FPT_Exchange_Data.DTO.Internal
{
    public class MomoOneTimePaymentRequest
    {
        public string PartnerCode { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public string OrderInfo { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
        public string IpnUrl { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public string ExtraData { get; set; } = string.Empty;
        public string Lang { get; set; } = "vi";
        public string Signature { get; set; } = string.Empty;

        public void MakeSignature(string accessKey, string secretKey)
        {
            var rawHash = "accessKey=" + accessKey +
                "&amount=" + this.Amount +
                "&extraData=" + this.ExtraData +
                "&ipnUrl=" + this.IpnUrl +
                "&orderId=" + this.OrderId +
                "&orderInfo=" + this.OrderInfo +
                "&partnerCode=" + this.PartnerCode +
                "&redirectUrl=" + this.RedirectUrl +
                "&requestId=" + this.RequestId +
                "&requestType=" + this.RequestType;
            Signature = HashHelper.HmacSHA256(rawHash, secretKey);
        }

        public (bool, string?) GetLink(string paymentUrl)
        {
            using HttpClient client = new HttpClient();
            var requestData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8,
                "application/json");

            var createPaymentLinkRes = client.PostAsync(paymentUrl, requestContent)
                .Result;

            if (createPaymentLinkRes.IsSuccessStatusCode)
            {
                var responseContent = createPaymentLinkRes.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert
                    .DeserializeObject<MomoOneTimePaymentCreateLinkViewModel>(responseContent);
                if (responseData.resultCode == "0")
                {
                    return (true, responseData.payUrl);
                }
                else
                {
                    return (false, responseData.message);
                }

            }
            else
            {
                return (false, createPaymentLinkRes.ReasonPhrase);
            }
        }
    }
}
