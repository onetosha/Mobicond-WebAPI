using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DBContext _dbContext;
        public EmployeeRepository(DBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task Create(Employee entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var sql = "INSERT INTO employees (UserId, Surname, GivenName, MiddleName, DeptId, PosId) VALUES (@UserId, @Surname, @GivenName, @MiddleName, @DeptId, @PosId)";
                await connection.ExecuteAsync(sql, new { UserId = entity.UserId, Surname = entity.Surname, GivenName = entity.GivenName, MiddleName = entity.MiddleName, DeptId = entity.DeptId, PosId = entity.PosId });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var sql = "DELETE FROM employees WHERE id = @Id";
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<Employee> Get(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "SELECT * FROM employees WHERE id = @Id";
                var employee = await connection.QueryFirstOrDefaultAsync<Employee>(query, new { Id = id });
                return employee;
            }
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "SELECT * FROM employees";
                var employees = await connection.QueryAsync<Employee>(query);
                return employees;
            }
        }

        public async Task Update(Employee entity)
        {
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = @"
                            UPDATE employees 
                            SET 
                                UserId = @UserId, 
                                Surname = @Surname, 
                                GivenName = @GivenName, 
                                MiddleName = @MiddleName, 
                                DeptId = @DeptId,
                                PosId = @PosId
                            WHERE Id = @Id";
                    await connection.ExecuteAsync(query, new { UserId = entity.UserId, Surname = entity.Surname, GivenName = entity.GivenName, MiddleName = entity.MiddleName, DeptId = entity.DeptId, PosId = entity.PosId });
                }
            }
        }
    }
}
