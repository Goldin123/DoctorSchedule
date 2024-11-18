using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Commands;
using DoctorSchedule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.CommandHandlers.Implementation
{
    public class UpdateAttendeeCommadHandler : IUpdateAttendeeCommadHandler
    {
        public UpdateAttendeeCommadHandler() { }

        public async Task<Attendee> Handle(Guid attendeeId, CreateAttendeeCommand command)
        {
            try
            {
                return new Attendee
                {
                    Id = attendeeId,
                    Name = command.Name,
                    Email = command.Email,
                    IsAttending = command.IsAttending ?? false
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

    }
}
