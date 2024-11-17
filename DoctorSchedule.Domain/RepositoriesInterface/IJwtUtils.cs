using DoctorSchedule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.RepositoriesInterface
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public int? ValidateJwtToken(string? token);
    }
}
