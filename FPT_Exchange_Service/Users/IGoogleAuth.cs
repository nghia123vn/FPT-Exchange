using FPT_Exchange_Data.DTO.Internal;

namespace FPT_Exchange_Service.Users
{
    public interface IGoogleAuth
    {
        public Task<string> WriteGoogleInfoAsync(UserGoogleInfo principal);

        public Task<UserGoogleInfo> GetUserInfoFromGoogle(string clientId, string clientSecret, string authorizationCode, string redirectUrl);
    }
}
