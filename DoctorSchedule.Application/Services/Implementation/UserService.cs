using DoctorSchedule.Application.Queries;
using DoctorSchedule.Application.Services.Interface;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Domain.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IJwtUtils _jwtUtils;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IJwtUtils jwtUtils, IUserRepository userRepository, ILogger<UserService> logger)
        {
            _jwtUtils = jwtUtils;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<AuthenticateResponse?> Authenticate(LoginUserQuery loginUserQuery)
        {
            try
            {
                var user = await _userRepository.AuthenticateUserAsync(loginUserQuery.Username, loginUserQuery.Password);
                if (user == null) return null;

                var token = _jwtUtils.GenerateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
            catch (Exception ex)
            {

                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _userRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}

