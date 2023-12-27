
using FPT_Exchange_Utility.Settings;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FPT_Exchange_Utility.Helpers
{
    public class ApiHelper
    {

        public static async Task<HttpResponseMessage> CreateWallet(Guid userId)
        {
            string url = AppEvironment.WALLET_API_DOMAIN + AppEvironment.WALLET_API_PATH_CREATE;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var data = new
                    {
                        UserId = userId.ToString(),
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(url, content);
                    return result;
                }
                catch (Exception e)
                {
                    return null;
                }

            }

        }

        public static async Task<(int?, bool)> UpdateWallet(string userId, int addScore)
        {
            string url = AppEvironment.WALLET_API_DOMAIN + AppEvironment.WALLET_API_PATH_UPDATE;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var data = new
                    {
                        UserId = userId,
                        RequestScore = addScore
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync(url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    JObject responseObject = JObject.Parse(responseContent);
                    return ((int)responseObject["data"]["currentScore"], response.IsSuccessStatusCode);

                }
                catch (Exception e)
                {
                    return (null, false);
                }

            }

        }

        public static async Task<string> GenerateGoogleOauth(object authenticationResult)
        {
            string url = AppEvironment.GATEWAY_API_DOMAIN + AppEvironment.GATEWAY_API_PATH_GOOGLE_AUTH;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var data = authenticationResult;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        JObject responseObject = JObject.Parse(responseContent);
                        return (string)responseObject["data"]["accessToken"];
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

        }

        public static async Task<HttpResponseMessage> GetUser(string id)
        {
            string url = AppEvironment.USER_API_DOMAIN + AppEvironment.USER_API_PATH_GET_ONE;
            try
            {
                return await AppEvironment.httpClient.GetAsync(url + $"/{id}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task SendNotify(string sendToUserId, string desc)
        {
            string url = AppEvironment.NOTIFICATION_API_DOMAIN + AppEvironment.NOTIFICATION_API_PATH_SEND;
            var data = new
            {
                Description = desc,
                SendTo = sendToUserId
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await AppEvironment.httpClient.PostAsync(url, content);
        }

    }
}
