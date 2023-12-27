using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.Wallets
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(FptExchangeDbContext context) : base(context)
        {
        }
    }
}
