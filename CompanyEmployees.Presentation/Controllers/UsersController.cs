using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{

    [ApiController]
    [Route("api/authentication")]
    public class UsersController : ControllerBase
    {
        private readonly IServiceManager _service;
        public UsersController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost]
        [DtoValidationFilter]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var identityUser = await _service.AuthenticationService.RegisterUser(userForRegistration);
            if (!identityUser.Succeeded)
            {
                foreach (var error in identityUser.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        [DtoValidationFilter]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _service.AuthenticationService.ValidateUser(user))
                return Unauthorized();

            var tokenDto = await _service.AuthenticationService.CreateToken(populateExpiration: true);
            return Ok(tokenDto);

        }
    }
}
