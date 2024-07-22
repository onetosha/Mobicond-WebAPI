using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Models.Enums;
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
                const string query = "INSERT INTO hierarchy (Name, Type, ParentId, DeptId) VALUES (@Name, @Type, @ParentId, @DeptId)";
                await connection.ExecuteAsync(query, node);
            }
        }

        //Удаление узла, если deleteChildren, то с его потомками, если нет - потомки переподвяжутся к родителю удаляемого узла (он просто выпадет из цепочки)
        public async Task DeleteNode(int id, bool deleteChildren)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                if (deleteChildren)
                {
                    const string query = @"WITH RECURSIVE DeleteCTE AS (
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
                    const string updateQuery = "UPDATE hierarchy SET ParentId = (SELECT ParentId FROM hierarchy WHERE Id = @Id) WHERE ParentId = @Id";
                    await connection.ExecuteAsync(updateQuery, new { Id = id });

                    const string deleteQuery = "DELETE FROM hierarchy WHERE Id = @Id";
                    await connection.ExecuteAsync(deleteQuery, new { Id = id });
                }
            }
        }
        
        public async Task<IEnumerable<HierarchyNode>> GetHierarchyForDept(int deptId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"WITH RECURSIVE HierarchyCTE AS (
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
        //Вспомогательный метод, получение типа узла
        public async Task<HierarchyType> GetNodeType(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT Type FROM hierarchy WHERE id = @id";
                var typeStr = await connection.ExecuteScalarAsync<string>(query, new { id } );
                Enum.TryParse<HierarchyType>(typeStr, out var type);
                return type;
            }
        }
        //Вспомогательный метод, получение Id организации родителя
        public async Task<int> GetParentDeptId(int? parentId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT DeptId FROM hierarchy WHERE Id = @ParentId";
                var deptId = await connection.ExecuteScalarAsync<int>(query, new { ParentId = parentId });
                return deptId;
            }
        }
        //Получение родительской иерархии для узла (т.е. дерево выше него, его предки)
        public async Task<IEnumerable<HierarchyNode>> GetParentHierarchy(int nodeId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"WITH RECURSIVE HierarchyCTE AS (
                    SELECT Id, Name, Type, ParentId, DeptId
                    FROM hierarchy
                    WHERE Id = @NodeId
                    UNION ALL
                    SELECT h.Id, h.Name, h.Type, h.ParentId, h.DeptId
                    FROM hierarchy h
                    INNER JOIN HierarchyCTE cte ON h.Id = cte.ParentId
                )
                SELECT * FROM HierarchyCTE";

                var hierarchyData = await connection.QueryAsync<HierarchyNode>(query, new { NodeId = nodeId });

                var lookup = hierarchyData.ToLookup(x => x.ParentId);
                foreach (var node in hierarchyData)
                {
                    node.Children = lookup[node.Id].ToList();
                }

                return lookup[null].ToList();
            }
        }

        public async Task UpdateNode(HierarchyNode node)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
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
