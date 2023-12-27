using FPT_Exchange_Data.DTO.Internal;
using FPT_Exchange_Service.Users;
using FPT_Exchange_Utility.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.OAuth.Model;

namespace OAuth_Server.Controllers
{
    [Route("api/oauth")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly GoogleAuthSettings _googleAuthSettings;
        private readonly IGoogleAuth _googleAuth;

        public OAuthController(IOptions<GoogleAuthSettings> googleAuthSettings, IGoogleAuth googleAuth)
        {
            _googleAuthSettings = googleAuthSettings.Value;
            _googleAuth = googleAuth;
        }

        [HttpGet("google-oauth")]
        public async Task<IActionResult> LoginGoogle([FromQuery(Name = "code")] string authorizationCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(authorizationCode))
                {
                    UserGoogleInfo authenticationResult = await _googleAuth.GetUserInfoFromGoogle(_googleAuthSettings.ClientId, _googleAuthSettings.ClientSecret, authorizationCode, _googleAuthSettings.RedirectUrl);
                    string token = await ApiHelper.GenerateGoogleOauth(authenticationResult);
                    return Ok(token);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch
            {
                return StatusCode(500);
            }


        }
    }
}
