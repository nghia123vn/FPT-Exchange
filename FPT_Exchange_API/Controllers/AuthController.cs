 using FPT_Exchange_Data.DTO.Internal;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Service.Users;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FPT_Exchange_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppSetting _appSettings;


        public AuthController(IUserService userService, IOptions<AppSetting> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticatedUser([FromBody][Required] AuthRequest auth)
        {
            var customer = await _userService.AuthenticatedUser(auth);
            if (customer != null)
            {
                return Ok(customer);
            }
            else
            {
                return NotFound("Not Found This User");
            }
        }

        [HttpGet]
        [Route("current-staff")]
        public async Task<IActionResult> GetCurrentUser()
        {
            //var user = (AuthModel)HttpContext.Items["User"]!;
            Request.Headers.TryGetValue("Authorization", out var authHeader);
            var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

            ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
            var role = ClaimTypes.Role;


            if (user != null)
            {
                Guid userId = Guid.Parse(user.FindFirstValue("userId"));
                var result = await _userService.GetUser(userId);
                if (result is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
            }
            return Unauthorized("Unauthorized");
        }
    }
}
