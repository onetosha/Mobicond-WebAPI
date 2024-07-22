using Microsoft.AspNetCore.Mvc;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class JobTitleController : Controller
    {
        private readonly IJobTitleRepository _jobTitleRepository;

        public JobTitleController(IJobTitleRepository positionRepository)
        {
            _jobTitleRepository = positionRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateJobTitile([FromBody] JobTitle jobTitle)
        {
            try
            {
                await _jobTitleRepository.Create(jobTitle);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteJobTitle(int id)
        {
            try
            {
                await _jobTitleRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllJobTitles()
        {
            try
            {
                var objs = await _jobTitleRepository.GetAll();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetJobTitle(int id)
        {
            try
            {
                var obj = await _jobTitleRepository.Get(id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateJobTitle([FromBody] JobTitle jobTitle)
        {
            try
            {
                await _jobTitleRepository.Update(jobTitle);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
