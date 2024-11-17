using DoctorSchedule.Application.Queries;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.Services.Interface
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(LoginUserQuery loginUserQuery);

        Task<User?> GetByIdAsync(int id);
    }
}
