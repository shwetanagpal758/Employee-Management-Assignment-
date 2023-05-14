using Employee_Management.Models;
using Employee_Management.Models.Dto;
using Employee_Management.Respository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Employee_Management.Controllers
{

    [Route("api/SecurityAuth")]
    [ApiController]
    public class SecurityController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response;

        public SecurityController(IUserRepository userRepository)
            
        {
            _userRepository = userRepository;
            this._response = new ();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _userRepository.Login(model);
            if(loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            bool ifUserNameUnique =_userRepository.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique) {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }
            var user = await _userRepository.Register(model);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            
            return Ok(_response);

        }
    }
}
