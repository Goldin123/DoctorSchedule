using DoctorSchedule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.RepositoriesInterface
{
    public interface IUserRepository
    {
        Task<User?> AuthenticateUserAsync(string username, string password);
        Task<User?> GetByIdAsync(int id);
    }
}
