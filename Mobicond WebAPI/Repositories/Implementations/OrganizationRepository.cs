using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Repositories.Implementations
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly DBContext _dbContext;
        public OrganizationRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(Organization entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "INSERT INTO organizations (name) VALUES (@Name)";
                await connection.ExecuteAsync(query, new { entity.Name });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "DELETE FROM organizations WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<Organization> Get(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT * FROM organizations WHERE id = @Id";
                var organization = await connection.QueryFirstOrDefaultAsync<Organization>(query, new { Id = id });
                return organization;
            }
        }

        public async Task<IEnumerable<Organization>> GetAll()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT * FROM organizations";
                var organizations = await connection.QueryAsync<Organization>(query);
                return organizations;
            }
        }

        public async Task Update(Organization entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "UPDATE organizations SET Name = @Name WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Name = entity.Name, Id = entity.Id });
            }
        }
    }
}
