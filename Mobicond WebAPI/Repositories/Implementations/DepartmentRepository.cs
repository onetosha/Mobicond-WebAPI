using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DBContext _dbContext;
        public DepartmentRepository(DBContext dBContext) 
        {
            _dbContext = dBContext;
        }
        public async Task Create(Department entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "INSERT INTO departments (name, orgid) VALUES (@Name, @OrgId)";
                await connection.ExecuteAsync(query, new { entity.Name, entity.OrgId });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "DELETE FROM departments WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<Department> Get(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "SELECT * FROM departments WHERE id = @Id";
                var department = await connection.QueryFirstOrDefaultAsync<Department>(query, new { Id = id });
                return department;
            }
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "SELECT * FROM departments";
                var departments = await connection.QueryAsync<Department>(query);
                return departments;
            }
        }

        public async Task Update(Department entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = @"
                            UPDATE departments 
                            SET 
                                Name = @Name,
                                OrgId = @OrgId
                            WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Name = entity.Name, Id = entity.Id, OrgId = entity.OrgId });
            }
        }
    }
}
