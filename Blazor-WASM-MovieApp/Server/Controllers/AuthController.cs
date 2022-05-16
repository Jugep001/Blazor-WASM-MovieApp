using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blazor_WASM_MovieApp.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly AuthenticationService _authenticationService;

        public AuthController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody] string json)
        {
            try
            {
                AuthInput authInput = JsonConvert.DeserializeObject<AuthInput>(json);
                AuthInput newAuthInput = await _authenticationService.Login(authInput);
                authInput.IsAuthSuccessful = true;
                return Ok(newAuthInput);
            }
            catch (BusinessException ex)
            {
                return Unauthorized(ex.ExceptionMessageList);
            }


        }

        [HttpPost("/Register")]
        public async Task<IActionResult> RegisterUser([FromBody] string json)
        {
            AuthInput authInput = JsonConvert.DeserializeObject<AuthInput>(json);
            await _authenticationService.Register(authInput);

            return Ok();
        }

        [HttpPut("/UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] string json)
        {
            try
            {
                AuthInput authInput = JsonConvert.DeserializeObject<AuthInput>(json);
                _authenticationService.UpdateUser(authInput);
                return Ok();
            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }

        }

        [HttpDelete("/DeleteUser/{id}")]
        public async Task<IActionResult> DeletePerson(string id)
        {
            try
            {
                _authenticationService.DeleteUser(id);
                return Ok();
            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }
        }

        [HttpGet("/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _authenticationService.GetUsers();
            string json = JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet("/GetUser/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var users = _authenticationService.GetUser(id);
            string json = JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

    }
}
