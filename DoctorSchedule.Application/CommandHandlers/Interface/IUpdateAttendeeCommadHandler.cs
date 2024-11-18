using DoctorSchedule.Application.Commands;
using DoctorSchedule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.CommandHandlers.Interface
{
    public interface IUpdateAttendeeCommadHandler
    {
        Task<Attendee> Handle(Guid attendeeId, CreateAttendeeCommand command);
    }
}
