using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Repositories.Implementations
{
    public class HierarchyRepository : IHierarchyRepository
    {
        private readonly DBContext _dbContext;
        public HierarchyRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddNode(HierarchyNode node)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "INSERT INTO hierarchy (Name, Type, ParentId, DeptId) VALUES (@Name, @Type, @ParentId, @DeptId)";
                await connection.ExecuteAsync(query, node);
            }
        }

        public async Task DeleteNode(int id, bool deleteChildren)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                if (deleteChildren)
                {
                    var query = @"WITH RECURSIVE DeleteCTE AS (
                              SELECT Id FROM hierarchy WHERE Id = @Id
                              UNION ALL 
                              SELECT h.Id FROM hierarchy h
                              INNER JOIN DeleteCTE cte ON cte.Id = h.ParentId
                              ) 
                              DELETE FROM hierarchy WHERE Id IN (SELECT Id FROM DeleteCTE)";
                    await connection.ExecuteAsync(query, new { Id = id });
                }
                else
                {
                    var updateQuery = "UPDATE hierarchy SET ParentId = (SELECT ParentId FROM hierarchy WHERE Id = @Id) WHERE ParentId = @Id";
                    await connection.ExecuteAsync(updateQuery, new { Id = id });

                    var deleteQuery = "DELETE FROM hierarchy WHERE Id = @Id";
                    await connection.ExecuteAsync(deleteQuery, new { Id = id });
                }
            }
        }

        public async Task<IEnumerable<HierarchyNode>> GetHierarchy(int deptId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = @"WITH RECURSIVE HierarchyCTE AS (
                        SELECT Id, Name, Type, ParentId, DeptId
                        FROM hierarchy
                        WHERE ParentId IS NULL AND DeptId = @DeptId
                        UNION ALL
                        SELECT h.Id, h.Name, h.Type, h.ParentId, h.DeptId
                        FROM hierarchy h
                        INNER JOIN HierarchyCTE cte ON cte.Id = h.ParentId
                        WHERE h.DeptId = @DeptId
                    )
                    SELECT * FROM HierarchyCTE";
                var hierarchyData = await connection.QueryAsync<HierarchyNode>(query, new { DeptId = deptId });

                var lookup = hierarchyData.ToLookup(x => x.ParentId);
                foreach (var node in hierarchyData)
                {
                    node.Children = lookup[node.Id].ToList();
                }

                return lookup[null].ToList();
            }
        }

        public async Task<bool> HasChildren(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = "SELECT COUNT(*) FROM hierarchy WHERE ParentId = @Id";
                var count = await connection.ExecuteScalarAsync<int>(query, new { Id = id });
                return count > 0;
            }
        }
        //TODO: Продумать
        public async Task UpdateNode(HierarchyNode node)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = @"
                            UPDATE hierarchy
                            SET Name = @Name,
                                Type = @Type,
                                ParentId = @ParentId
                            WHERE Id = @Id";
                await connection.ExecuteAsync(query, node);
            }
        }
    }
}
