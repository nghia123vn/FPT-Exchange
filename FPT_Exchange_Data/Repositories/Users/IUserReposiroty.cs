using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Repositories.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindUserByEmailAsync(string email);
    }
}
