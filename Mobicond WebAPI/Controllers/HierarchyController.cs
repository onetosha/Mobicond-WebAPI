using Microsoft.AspNetCore.Mvc;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Implementations;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Controllers
{
    [Route("[controller]")]
    public class HierarchyController : Controller
    {
        private readonly IHierarchyRepository _hierarchyRepository;
        public HierarchyController(IHierarchyRepository hierarchyRepository) 
        {
            _hierarchyRepository = hierarchyRepository;
        }

        [HttpGet("GetHierarchy")]
        public async Task<IActionResult> GetHierarchy(int deptId)
        {
            try
            {
                var hierarchy = await _hierarchyRepository.GetHierarchy(deptId);
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddNode")]
        public async Task<IActionResult> AddNodeToHierarchy([FromBody] HierarchyNode node)
        {
            try
            {
                await _hierarchyRepository.AddNode(node);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("HasChildren")]
        public async Task<IActionResult> HasChildren(int nodeId)
        {
            try
            {
                bool result = await _hierarchyRepository.HasChildren(nodeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("DeleteNode")]
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

        [HttpPost("UpdateNode")]
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
