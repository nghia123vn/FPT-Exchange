using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.ProductActivities
{
    public class ProductActivityRepository : Repository<ProductActivy>, IProductActivityRepository
    {
        public ProductActivityRepository(FptExchangeDbContext context) : base(context)
        {
        }
    }
}
