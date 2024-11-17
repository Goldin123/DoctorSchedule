using DoctorSchedule.Application.Services.Interface;
using DoctorSchedule.Domain.Notifications;
using DoctorSchedule.Domain.RepositoriesInterface;

namespace DoctorSchedule.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var userId = jwtUtils.ValidateJwtToken(token);
                if (userId != null)
                {
                    context.Items["User"] = await userService.GetByIdAsync(userId.Value);
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                new Exception(Notification.GeneralExceptionMessage);
            }
        }

    }
}
