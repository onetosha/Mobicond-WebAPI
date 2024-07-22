using Dapper;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Repositories.Implementations
{
    public class RouteRepository : IRouteRepository
    {
        private readonly DBContext _dbContext;
        public RouteRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Models.Route entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "INSERT INTO routes (name, DeptId) VALUES (@Name, @DeptId)";
                await connection.ExecuteAsync(query, new { entity.Name, entity.DeptId });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "DELETE FROM routes WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<Models.Route> Get(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT * FROM routes WHERE id = @Id";
                var route = await connection.QueryFirstOrDefaultAsync<Models.Route>(query, new { Id = id });
                return route;
            }
        }

        public async Task<IEnumerable<Models.Route>> GetAll()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT * FROM routes";
                var routes = await connection.QueryAsync< Models.Route> (query);
                return routes;
            }
        }

        public async Task Update(Models.Route entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                                    UPDATE routes 
                                    SET
                                    Name = @Name,
                                    DeptId = @DeptId
                                    WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Name = entity.Name, Id = entity.Id, Deptid = entity.DeptId });
            }
        }

        //ЛОГИКА ДЛЯ ДОБАВЛЕНИЯ КОНТРОЛОВ

        //Вспомогательный метод, проверка существования контроля в маршруте
        public async Task<bool> CheckControlInRouteExists(int routeId, int controlId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "SELECT COUNT(1) FROM route_controls WHERE RouteId = @RouteId AND ControlId = @ControlId";
                var exists = await connection.ExecuteScalarAsync<bool>(query, new { RouteId = routeId, ControlId = controlId });
                return exists;
            }
        }
        public async Task AddControlToRoute(int routeId, int controlId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = "INSERT INTO route_controls (RouteId, ControlId) VALUES (@RouteId, @ControlId)";
                await connection.ExecuteAsync(query, new { RouteId = routeId, ControlId = controlId });
            }
        }

        public async Task DeleteControlFromRoute(int routeId, int controlId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"DELETE 
                                    FROM route_controls
                                    WHERE
                                    RouteId = @RouteId
                                    AND
                                    ControlId = @ControlId";
                await connection.ExecuteAsync(query, new { RouteId = routeId, ControlId = controlId });
            }
        }
        //Получение всех контролей в маршруте списком
        public async Task<IEnumerable<HierarchyNode>> GetRouteControls(int routeId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                SELECT h.Id, h.Name, h.Type, h.ParentId, h.DeptId
                FROM hierarchy h
                INNER JOIN route_controls rtp ON h.Id = rtp.ControlId
                WHERE rtp.RouteId = @RouteId";
                var controls = await connection.QueryAsync<HierarchyNode>(query, new { RouteId = routeId });
                return controls;
            }
        }
        //Получение всех контролей в маршруте деревом с их предками
        public async Task<IEnumerable<HierarchyNode>> GetRouteHierarchy(int routeId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                string sql = @"
                WITH RECURSIVE HierarchyCTE AS (
                    SELECT h.Id, h.Name, h.Type, h.ParentId, rc.RouteId
                    FROM hierarchy h
                    INNER JOIN route_controls rc ON h.Id = rc.ControlId
                    WHERE rc.RouteId = @RouteId
                    UNION ALL
                    SELECT h.Id, h.Name, h.Type, h.ParentId, cte.RouteId
                    FROM hierarchy h
                    INNER JOIN HierarchyCTE cte ON h.Id = cte.ParentId
                )
                SELECT DISTINCT h.Id, h.Name, h.Type, h.ParentId
                FROM HierarchyCTE h";
                var hierarchyData = await connection.QueryAsync<HierarchyNode>(sql, new { RouteId = routeId });
                var lookup = hierarchyData.ToLookup(x => x.ParentId);
                foreach (var node in hierarchyData)
                {
                    node.Children = lookup[node.Id].ToList();
                }

                return lookup[null].ToList();
            }
        }
    }
}
