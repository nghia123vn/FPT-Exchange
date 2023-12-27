using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Internal;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Service.Users;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FPT_Exchange_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoogleAuth _googleAuthService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSetting _appSettings;

        public UserController(IGoogleAuth googleAuthService, IUserService userService, SignInManager<IdentityUser> signInManager, IOptions<AppSetting> appSettings)
        {
            _googleAuthService = googleAuthService;
            _userService = userService;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }


        [HttpPost("signin-google")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> LoginGoogle([FromBody] UserGoogleInfo authenticationResult)
        {
            try
            {
                string issSuccess = await _googleAuthService.WriteGoogleInfoAsync(authenticationResult);
                if (issSuccess != null)
                {
                    return Ok(new { Data = new { AccessToken = issSuccess } });
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilterModel filter)
        {
            var result = await _userService.GetUsers(filter);
            if (result is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            return BadRequest("Something wrong!!!");
        }


        [HttpPost]
        [Route("register-staffs")]
        public async Task<IActionResult> RegisterStaff([FromBody] RegisterStaffRequest request)
        {
            var result = await _userService.RegisterStaff(request);
            if (result is JsonResult jsonResult)
            {
                return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
            }
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == StatusCodes.Status409Conflict)
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Email already in use.");
                }
                if (statusCodeResult.StatusCode == StatusCodes.Status404NotFound)
                {
                    return NotFound("Not found this station");
                }
                if (statusCodeResult.StatusCode == StatusCodes.Status400BadRequest)
                {
                    return BadRequest("Something wrong when save to database.");
                }
            }
            return BadRequest("Something wrong!!!");
        }

        [HttpPut]
        [Route("change-avatar")]
        public async Task<IActionResult> ChangeAvatar(IFormFile? avatar)
        {
            if (avatar == null || avatar.Length <= 0)
            {
                return BadRequest("Please choose image");
            }

            //var user = (AuthModel)HttpContext.Items["User"]!;
            Request.Headers.TryGetValue("Authorization", out var authHeader);
            var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

            ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
            Guid userId = Guid.Parse(user.FindFirstValue("userId"));
            if (user != null)
            {
                try
                {
                    var result = await _userService.AddAvatar(userId, avatar);
                    if (result is JsonResult jsonResult)
                    {
                        return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                    }
                    if (result is StatusCodeResult statusCodeResult)
                    {
                        var statusCode = statusCodeResult.StatusCode;
                        if (statusCode == StatusCodes.Status400BadRequest)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "Something wrong when save to database.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
                }
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("staffs/{id}")]
        public async Task<IActionResult> UpdateStaff([FromRoute] Guid id,
                                                        [FromBody] UpdateStaffRequest request)
        {
            var result = await _userService.UpdateStaff(id, request);
            if (result is JsonResult jsonResult)
            {
                return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
            }

            if (result is StatusCodeResult statusCodeResult)
            {
                var statusCode = statusCodeResult.StatusCode;
                if (statusCode == StatusCodes.Status404NotFound)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Customer not found or Station not found.");
                }

            }
            return BadRequest("Something wrong!!!");

        }

        [HttpDelete]
        [Route("staffs/{id}")]
        public async Task<IActionResult> DeleteStaff([FromRoute] Guid id)
        {
            var result = await _userService.DeleteStaff(id);
            if (result is StatusCodeResult statusCodeResult)
            {
                var statusCode = statusCodeResult.StatusCode;
                if (statusCode == StatusCodes.Status404NotFound)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Customer not found.");
                }
                if (statusCode == StatusCodes.Status204NoContent)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                if (statusCode == StatusCodes.Status400BadRequest)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Something wrong when save to database.");
                }
            }
            return BadRequest("Something wrong!!!");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                if (Guid.TryParse(id, out Guid resultGuid))
                {
                    var result = await _userService.GetUser(resultGuid);
                    if (result is JsonResult jsonResult)
                    {
                        return Ok(jsonResult.Value);
                    }
                }
                return BadRequest();

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

    }
}
