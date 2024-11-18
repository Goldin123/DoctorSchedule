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
    public class CreateAttendeeCommandHandler : ICreateAttendeeCommandHandler
    {
        public CreateAttendeeCommandHandler() { }
        public async Task<Attendee> Handle(Guid eventId, CreateAttendeeCommand command)
        {
            try
            {
                return new Attendee
                {
                    Id = Guid.NewGuid(),
                    Name = command.Name,
                    Email = command.Email,
                    IsAttending = command.IsAttending ?? false,
                    EventId = eventId
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
