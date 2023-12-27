using AutoMapper;
using FPT_Exchange_Data;
using FPT_Exchange_Data.DTO.Internal;
using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Repositories.Users;
using FPT_Exchange_Utility.Constants;
using FPT_Exchange_Utility.Extensions;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace FPT_Exchange_Service.Users
{
    public class GoogleAuth : BaseService, IGoogleAuth
    {
        private readonly AppSetting _appSettings;

        private readonly IUserRepository _userRepository;

        public GoogleAuth(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _userRepository = unitOfWork.User;
        }

        public async Task<UserGoogleInfo> GetUserInfoFromGoogle(string clientId, string clientSecret, string authorizationCode, string redirectUri)
        {
            if (!string.IsNullOrEmpty(authorizationCode))
            {
                string tokenEndpoint = "https://oauth2.googleapis.com/token";

                using (var httpClient = new HttpClient())
                {
                    var tokenRequestContent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("code", authorizationCode),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("grant_type", "authorization_code")
                });

                    var tokenResponse = await httpClient.PostAsync(tokenEndpoint, tokenRequestContent);
                    var tokenResponseString = await tokenResponse.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(tokenResponseString);


                    string accessToken = (string)jsonObject["access_token"];


                    using (var userInfoClient = new HttpClient())
                    {
                        userInfoClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        var userInfoEndpoint = "https://www.googleapis.com/oauth2/v1/userinfo";
                        var userInfoResponse = await userInfoClient.GetAsync(userInfoEndpoint);
                        userInfoResponse.EnsureSuccessStatusCode();

                        var userInfoString = await userInfoResponse.Content.ReadAsStringAsync();

                        dynamic userInfo = JObject.Parse(userInfoString);
                        UserGoogleInfo _userInfo = new UserGoogleInfo
                        {
                            Name = userInfo.name,
                            Email = userInfo.email,
                            ProfilePictureUrl = userInfo.picture
                        };

                        return _userInfo;
                    }

                }
            }
            else
            {
                return null;
            }

        }

        public async Task<string> WriteGoogleInfoAsync(UserGoogleInfo principal)
        {
            try
            {
                bool flag = false;
                var thisUser = await _userRepository.FindUserByEmailAsync(principal.Email);

                if (thisUser == null)
                {
                    //Create new if doesn't exist
                    Guid roleId = Guid.Parse(UserRole.Customer.GetEnumMember());
                    flag = true;
                    thisUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = principal.Email,
                        Name = principal.Name,
                        Avatar = principal.ProfilePictureUrl,
                        Password = "",
                        Status = UserStatus.Active.ToString(),
                        RoleId = roleId
                    };



                    _userRepository.Add(thisUser);
                }
                else
                {
                    thisUser.Name = principal.Name;
                    thisUser.Avatar = principal.ProfilePictureUrl;
                    //Update info if exist
                    if (thisUser.Status == UserStatus.DeActive.ToString())
                    {
                        return null;
                    }
                }


                await _userRepository.SaveChangesAsync();

                if (flag)
                {
                    HttpResponseMessage response = await ApiHelper.CreateWallet(thisUser.Id);
                }

                return JwtHelper.GenerateJwtToken(thisUser.Id.ToString(), thisUser.Name, thisUser.RoleId.ToString(), _appSettings.Secret); ;
            }
            catch
            {
                return null;
            }
        }



    }
}
