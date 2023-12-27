using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(FptExchangeDbContext context) : base(context)
        {

        }
    }
}
