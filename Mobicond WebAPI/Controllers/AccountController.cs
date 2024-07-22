using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mobicond_WebAPI.Helpers.Jwt;
using Mobicond_WebAPI.Models;
using Mobicond_WebAPI.Repositories.Interfaces;

namespace Mobicond_WebAPI.Controllers
{
    //TODO: Routing
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        //TODO: Logout
        public AccountController(IUserRepository userRepository, IOptions<JwtSettings> options)
        {
            _userRepository = userRepository;
            _jwtSettings = options.Value;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                //Запрашиваем пользователя из БД
                var existingUser = await _userRepository.GetUserByNameAsync(model.Username);
                //Если пользователя нет, или неправильный пароль, то
                if (existingUser == null || !BCrypt.Net.BCrypt.Verify(model.Password, existingUser.PasswordHash))
                {
                    return Unauthorized();
                }
                //Генерируем токен 
                var token = JwtTokenExtension.GenerateJWTToken(existingUser, _jwtSettings);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            try
            {
                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
                };
                //Создаем пользователя с захешированным паролем
                await _userRepository.CreateUserAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //TODO: Придумать logout (пока предложил бы просто удалять токен у клиента...)
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
