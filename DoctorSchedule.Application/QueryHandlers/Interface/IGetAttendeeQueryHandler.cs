using DoctorSchedule.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.QueryHandlers.Interface
{
    public interface IGetAttendeeQueryHandler
    {
        Task<AttendeeResponse> HandleAsync(Guid eventId, Guid attendeeId);
    }
}
