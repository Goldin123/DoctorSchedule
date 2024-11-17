using DoctorSchedule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.RepositoriesInterface
{
    /// <summary>
    /// This interfaces all the JWT functionality required by the application.
    /// </summary>
    public interface IJwtUtils
    {
        /// <summary>
        /// This defines how the JWT token will be generated.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateJwtToken(User user);
        /// <summary>
        /// This defines how the JWT token will be validated.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int? ValidateJwtToken(string? token);
    }
}
