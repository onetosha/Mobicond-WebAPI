using Microsoft.AspNetCore.Mvc;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Implementations;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                await _employeeRepository.Create(employee);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _employeeRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var objs = await _employeeRepository.GetAll();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var obj = await _employeeRepository.Get(id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            try
            {
                await _employeeRepository.Update(employee);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
