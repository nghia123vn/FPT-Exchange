using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.Stations
{
    public class StationRepository : Repository<Station>, IStationRepository
    {
        public StationRepository(FptExchangeDbContext context) : base(context)
        {
        }
    }
}
