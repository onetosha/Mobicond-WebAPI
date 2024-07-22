using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Controllers
{
    [Route("/api/v1/[controller]")]
    [Authorize(Roles = "SU")]
    [ApiController]
    public class OrganizationController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationController(IDepartmentRepository departmentRepository, IOrganizationRepository organizationRepository)
        {
            _departmentRepository = departmentRepository;
            _organizationRepository = organizationRepository;
        }
        ///////////////////////////////////////////////////////////////////////ORGANIZATION
        [HttpPost("create-organization")]
        public async Task<IActionResult> CreateOrganization([FromBody] Organization organization)
        {
            try
            {
                await _organizationRepository.Create(organization);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-organization")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            try
            {
                await _organizationRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all-organizations")]
        public async Task<IActionResult> GetAllOrganizations()
        {
            try
            {
                var objs = await _organizationRepository.GetAll();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-organization")]
        public async Task<IActionResult> GetOrganization(int id)
        {
            try
            {
                var obj = await _organizationRepository.Get(id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-organization")]
        public async Task<IActionResult> UpdateOrganization([FromBody] Organization organization)
        {
            try
            {
                await _organizationRepository.Update(organization);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////DEPARTMENT
        [HttpPost("create-department")]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            try
            {
                await _departmentRepository.Create(department);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-department")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                await _departmentRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all-departments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var objs = await _departmentRepository.GetAll();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-department")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            try
            {
                var obj = await _departmentRepository.Get(id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-department")]
        public async Task<IActionResult> UpdateDepartment([FromBody] Department department)
        {
            try
            {
                await _departmentRepository.Update(department);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
