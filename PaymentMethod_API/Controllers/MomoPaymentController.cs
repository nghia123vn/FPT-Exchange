using FPT_Exchange_Data.DTO.Request.Get;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Service.Payment;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PaymentMethod_API.Controllers
{
    [Route("api/payment-methods/momo")]
    [ApiController]
    public class MomoPaymentController : ControllerBase
    {
        private readonly IMomoPaymentService _momoPaymentService;
        private readonly AppSetting _appSettings;

        public MomoPaymentController(IMomoPaymentService momoPaymentService, IOptions<AppSetting> appSettings)
        {
            _momoPaymentService = momoPaymentService;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody][Required] MomoPaymentRequest req)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                var res = _momoPaymentService.Create(req, user.FindFirstValue("userId"));
                if (res == null)
                {
                    return BadRequest();
                }
                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        [HttpGet("return/{id}")]
        public async Task<IActionResult> MomoReturn([FromQuery][Required] MomoPaymentResultRequest req, string id)
        {
            try
            {
                var res = await _momoPaymentService.HandleResult(req, id);
                if (res == null)
                {
                    return BadRequest();
                }
                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }
    }
}
