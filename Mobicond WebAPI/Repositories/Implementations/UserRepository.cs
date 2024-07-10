using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;
        public UserRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateUserAsync(User user)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "INSERT INTO users (username, passwordhash, rolecode) VALUES (@Username, @PasswordHash, @RoleCode)";
                await connection.ExecuteAsync(query, new { user.Username, user.PasswordHash, user.RoleCode });
            }
        }
        public async Task<User> GetUserByNameAsync(string username)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "SELECT * FROM users WHERE username = @Username";
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
                return user;
            }
        }
    }
}
