using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Commands;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.CommandHandlers.Implementation
{
    public class CreateEventCommandHandler : ICreateEventCommandHandler
    {
        private readonly ILogger<CreateEventCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        public CreateEventCommandHandler(ILogger<CreateEventCommandHandler> logger, IEventRepository eventRepository ) 
        {
            _logger = logger;
            _eventRepository = eventRepository;
        }

        /// <summary>
        /// This method is responsible handle commands that create an event.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Event> HandleAsync(CreateEventCommand command) 
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: attempting to add an event {command.Title}.");

                var newEvent = new Event
                {
                    Id = Guid.NewGuid(),
                    Title = command.Title,
                    Description = command.Description,
                    StartTime = command.StartTime ?? DateTime.UtcNow.AddDays(2),
                    EndTime = command.EndTime ?? DateTime.UtcNow.AddDays(2).AddHours(1),
                    Attendees = command.Attendees
                };

                if(await _eventRepository.CreateEventAsync(newEvent)) 
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: successfully added an event {command.Title} with ({command.Attendees?.Count()}) attendees.");
                    return newEvent;
                }
                else 
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to add an event {command.Title}.");
                    return null;
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to add an event {command.Title}. {ex.Message}.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
