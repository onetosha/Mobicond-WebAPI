using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces.Base;

namespace Mobicond_WebAPI.Repositories.Interfaces
{
    public interface IRouteRepository : IBaseRepository<Models.Route>
    {
        Task AddControlToRoute(int routeId, int controlId);
        Task DeleteControlFromRoute(int routeId, int controlId);
        Task<IEnumerable<HierarchyNode>> GetRouteControls(int routeId);
        Task<IEnumerable<HierarchyNode>> GetRouteHierarchy(int routeId);
        Task<bool> CheckControlInRouteExists(int routeId, int controlId);
    }
}
