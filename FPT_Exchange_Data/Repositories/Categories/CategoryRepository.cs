using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.Categories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(FptExchangeDbContext context) : base(context)
        {
        }
    }
}
