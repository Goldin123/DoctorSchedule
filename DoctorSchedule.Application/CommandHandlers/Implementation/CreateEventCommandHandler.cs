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
    public class CreateEventCommandHandler : ICreateEventCommandHandler
    {
        public CreateEventCommandHandler() { }

        public async Task<Event> Handle(CreateEventCommand command) 
        {
            try
            {
                return new Event
                {
                    Id = Guid.NewGuid(),
                    Title = command.Title,
                    Description = command.Description,
                    StartTime = command.StartTime ?? DateTime.UtcNow.AddDays(2),
                    EndTime = command.EndTime ?? DateTime.UtcNow.AddDays(2).AddHours(1),
                    Attendees = command.Attendees
                };

            }
            catch (Exception ex) 
            {
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
