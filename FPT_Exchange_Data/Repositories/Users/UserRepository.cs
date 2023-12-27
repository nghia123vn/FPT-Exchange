using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.Users
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FptExchangeDbContext context) : base(context)
        {
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            var queryUser = (from user in GetAll()
                             where user.Email == email
                             select user);
            return await Task.Run(() => queryUser.FirstOrDefault());
        }

    }
}
