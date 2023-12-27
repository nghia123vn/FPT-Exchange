using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Exchange_Service.Wallets
{
    public interface IWalletService
    {
        public Task<IActionResult> CreateUserWalletAsync(Guid userId);
        public Task<Wallet> AddScore(AddScoreRequest request);

    }
}
