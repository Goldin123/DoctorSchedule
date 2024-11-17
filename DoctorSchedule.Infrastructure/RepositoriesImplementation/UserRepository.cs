using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Infrastructure.RepositoriesImplementation
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ILogger<UserRepository> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);

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
                return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
