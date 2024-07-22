using Microsoft.AspNetCore.Mvc;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Models.Enums;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class RouteController : Controller
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IHierarchyRepository _hierarchyRepository;
        public RouteController(IRouteRepository routeRepository, IHierarchyRepository hierarchyRepository)
        {
            _routeRepository = routeRepository;
            _hierarchyRepository = hierarchyRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoute([FromBody] Models.Route route)
        {
            try
            {
                await _routeRepository.Create(route);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            try
            {
                await _routeRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoutes()
        {
            try
            {
                var objs = await _routeRepository.GetAll();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetRoute(int id)
        {
            try
            {
                var obj = await _routeRepository.Get(id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateRoute([FromBody] Models.Route route)
        {
            try
            {
                await _routeRepository.Update(route);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //TODO: Проверять, в том ли она подразделении
        [HttpPost("add-control")]
        public async Task<IActionResult> AddControl(int routeId, int controlId)
        {
            try
            {
                //Получаем тип контроля из репозитория иерархии (т.к. контроль - часть иерархии)
                HierarchyType type =  await _hierarchyRepository.GetNodeType(controlId);
                //Если тип элемента - не контроль (а объект, узел, и т.п.)
                if (type != HierarchyType.Control)
                {
                    return BadRequest("Элемент должен быть Control");
                }
                //Проверяем, существует ли контроль в маршруте
                var exists = await _routeRepository.CheckControlInRouteExists(routeId, controlId);
                if (exists)
                {
                    return BadRequest("Данный контроль уже существует в маршруте");
                }
                await _routeRepository.AddControlToRoute(routeId, controlId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-control")]
        public async Task<IActionResult> DeleteControl(int routeId, int controlId)
        {
            try
            {
                await _routeRepository.DeleteControlFromRoute(routeId, controlId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-controls")]
        public async Task<IActionResult> GetRouteControls(int routeId)
        {
            try
            {
                var controls = await _routeRepository.GetRouteControls(routeId);
                return Ok(controls);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-hierarchy")]
        public async Task<IActionResult> GetRouteHierarchy(int routeId)
        {
            try
            {
                var hierarchy = await _routeRepository.GetRouteHierarchy(routeId);
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
