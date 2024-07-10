using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "SU")]
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
        [HttpPost("CreateOrganization")]
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

        [HttpDelete("DeleteOrganization")]
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

        [HttpGet("AllOrganizations")]
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

        [HttpGet("GetOrganization")]
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

        [HttpPost("UpdateOrganization")]
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
        [HttpPost("CreateDepartment")]
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

        [HttpDelete("DeleteDepartment")]
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

        [HttpGet("AllDepartments")]
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

        [HttpGet("GetDepartment")]
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

        [HttpPost("UpdateDepartment")]
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
