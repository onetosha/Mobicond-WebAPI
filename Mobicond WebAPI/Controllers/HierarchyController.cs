using Microsoft.AspNetCore.Mvc;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Implementations;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class HierarchyController : Controller
    {
        private readonly IHierarchyRepository _hierarchyRepository;
        public HierarchyController(IHierarchyRepository hierarchyRepository) 
        {
            _hierarchyRepository = hierarchyRepository;
        }
        [HttpGet("get-dept-hierarchy")]
        public async Task<IActionResult> GetHierarchyForDept(int deptId)
        {
            try
            {
                var hierarchy = await _hierarchyRepository.GetHierarchyForDept(deptId);
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-parent-hierarchy")]
        public async Task<IActionResult> GetParentHierarchy(int nodeId)
        {
            try
            {
                var hierarchy = await _hierarchyRepository.GetParentHierarchy(nodeId);
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-node")]
        public async Task<IActionResult> AddNodeToHierarchy([FromBody] HierarchyNode node)
        {
            try
            {
                //Если DeptId в запросе не указан и элемент не корневой в иерархии, даем DeptId родителя
                if (node.ParentId != null && (node.DeptId == 0 || node.DeptId == null))
                {
                    var deptId = await _hierarchyRepository.GetParentDeptId(node.ParentId);
                    node.DeptId = deptId;
                }
                await _hierarchyRepository.AddNode(node);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("delete-node")]
        public async Task<IActionResult> DeleteNode(int nodeId, bool deleteChildren)
        {
            try
            {
                await _hierarchyRepository.DeleteNode(nodeId, deleteChildren);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-node")]
        public async Task<IActionResult> UpdateNode([FromBody] HierarchyNode node)
        {
            try
            {
                await _hierarchyRepository.UpdateNode(node);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
