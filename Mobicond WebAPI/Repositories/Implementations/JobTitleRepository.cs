using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Repositories.Implementations
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly DBContext _dbContext;
        public JobTitleRepository(DBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task Create(JobTitle jobTitile)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "INSERT INTO jobtitles (name) VALUES (@Name)";
                await connection.ExecuteAsync(query, new { jobTitile.Name });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "DELETE FROM jobtitles WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<JobTitle> Get(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT * FROM jobtitles WHERE id = @Id";
                var position = await connection.QueryFirstOrDefaultAsync<JobTitle>(query, new { Id = id });
                return position;
            }
        }

        public async Task<IEnumerable<JobTitle>> GetAll()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT * FROM jobtitles";
                var positions = await connection.QueryAsync<JobTitle>(query);
                return positions;
            }
        }

        public async Task Update(JobTitle jobTitile)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "UPDATE jobtitles SET Name = @Name WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Name = jobTitile.Name, Id = jobTitile.Id });
            }
        }
    }
}
