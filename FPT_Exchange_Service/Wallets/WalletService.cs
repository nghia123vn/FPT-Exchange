using AutoMapper;
using FPT_Exchange_Data;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Repositories.Wallets;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FPT_Exchange_Service.Wallets
{
    public class WalletService : BaseService, IWalletService
    {
        private readonly AppSetting _appSettings;

        private readonly IWalletRepository _walletRepository;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _walletRepository = unitOfWork.Wallet;
        }

        public async Task<Wallet> AddScore(AddScoreRequest request)
        {
            var findWallet = _walletRepository.FirstOrDefault(x => request.UserId == x.UserId.ToString());
            if (findWallet == null)
            {
                return null;
            }
            findWallet.Score += request.RequestScore;
            _walletRepository.Update(findWallet);
            await _walletRepository.SaveChangesAsync();
            return findWallet;

        }

        public async Task<IActionResult> CreateUserWalletAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            _walletRepository.Add(new Wallet
            {
                UserId = userId,
                Id = Guid.NewGuid(),
                Score = 0,
            });

            await _walletRepository.SaveChangesAsync();
            return new StatusCodeResult(StatusCodes.Status200OK);


        }
    }
}
