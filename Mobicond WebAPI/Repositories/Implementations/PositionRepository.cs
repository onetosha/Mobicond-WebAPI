using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Repositories.Implementations
{
    public class PositionRepository : IPositionRepository
    {
        private readonly DBContext _dbContext;
        public PositionRepository(DBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task Create(Position position)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "INSERT INTO positions (name) VALUES (@Name)";
                await connection.ExecuteAsync(query, new { position.Name });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "DELETE FROM positions WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<Position> Get(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "SELECT * FROM positions WHERE id = @Id";
                var position = await connection.QueryFirstOrDefaultAsync<Position>(query, new { Id = id });
                return position;
            }
        }

        public async Task<IEnumerable<Position>> GetAll()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "SELECT * FROM positions";
                var positions = await connection.QueryAsync<Position>(query);
                return positions;
            }
        }

        public async Task Update(Position position)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "UPDATE positions SET Name = @Name WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Name = position.Name, Id = position.Id });
            }
        }
    }
}
