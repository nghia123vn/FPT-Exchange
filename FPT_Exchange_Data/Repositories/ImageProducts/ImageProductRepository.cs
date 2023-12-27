using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.ImageProducts
{
    public class ImageProductRepository : Repository<ImageProduct>, IImageProductRepository
    {
        public ImageProductRepository(FptExchangeDbContext context) : base(context)
        {

        }
    }
}
