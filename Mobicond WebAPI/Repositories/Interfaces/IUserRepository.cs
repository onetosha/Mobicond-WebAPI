using Mobicond_WebAPI.Models;

namespace Mobicond_WebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByNameAsync(string username);
        Task CreateUserAsync(User user);
    }
}
