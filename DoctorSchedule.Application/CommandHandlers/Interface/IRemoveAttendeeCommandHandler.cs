using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.CommandHandlers.Interface
{
    public interface IRemoveAttendeeCommandHandler
    {
        Task<bool> HandleAsync(Guid eventId, Guid attendeeId);
    }
}
