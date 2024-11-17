using DoctorSchedule.Application.Queries;
using DoctorSchedule.Application.Services.Interface;
using DoctorSchedule.Domain.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginUserQuery model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = Notification.UserPasswordIncorrectMessage });

            return Ok(response);
        }
    }
}
