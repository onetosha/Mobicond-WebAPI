using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces.Base;

namespace Mobicond_WebAPI.Repositories.Interfaces
{
    public interface IHierarchyRepository
    {
        Task AddNode(HierarchyNode node);
        Task DeleteNode(int id, bool deleteChildren);
        Task UpdateNode(HierarchyNode node);
        Task<IEnumerable<HierarchyNode>> GetHierarchy(int deptId);
        Task<bool> HasChildren(int id);
    }
}
