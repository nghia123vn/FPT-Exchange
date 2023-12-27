using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Service.Wallets;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Wallet_API.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("kz9ijalm6067ACxopm")]//CREATE
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest request)
        {
            try
            {
                return await _walletService.CreateUserWalletAsync(Guid.Parse(request.userId));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("os19clk52homlachjak")]
        public async Task<IActionResult> AddScore([FromBody][Required] AddScoreRequest request)
        {
            try
            {
                var result = await _walletService.AddScore(request);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(new
                {
                    Data = new
                    {
                        CurrentScore = result.Score
                    },
                    Message = "Update success"
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
