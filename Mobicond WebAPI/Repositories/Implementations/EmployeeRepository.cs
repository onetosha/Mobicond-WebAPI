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
                const string query = "INSERT INTO employees (UserId, Surname, GivenName, MiddleName, DeptId, JobTitleId) VALUES (@UserId, @Surname, @GivenName, @MiddleName, @DeptId, @JobTitleId)";
                await connection.ExecuteAsync(query, new { UserId = entity.UserId, Surname = entity.Surname, GivenName = entity.GivenName, MiddleName = entity.MiddleName, DeptId = entity.DeptId, PosId = entity.JobTitleId });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "DELETE FROM employees WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<Employee> Get(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT * FROM employees WHERE id = @Id";
                var employee = await connection.QueryFirstOrDefaultAsync<Employee>(query, new { Id = id });
                return employee;
            }
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT * FROM employees";
                var employees = await connection.QueryAsync<Employee>(query);
                return employees;
            }
        }

        public async Task Update(Employee entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                            UPDATE employees 
                            SET 
                                UserId = @UserId, 
                                Surname = @Surname, 
                                GivenName = @GivenName, 
                                MiddleName = @MiddleName, 
                                DeptId = @DeptId,
                                JobTitleId = @JobTitleId
                            WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { UserId = entity.UserId, Surname = entity.Surname, GivenName = entity.GivenName, MiddleName = entity.MiddleName, DeptId = entity.DeptId, JobTitleId = entity.JobTitleId });

            }
        }
    }
}
