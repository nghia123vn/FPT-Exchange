using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.Transactions
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(FptExchangeDbContext context) : base(context)
        {
        }
    }
}
