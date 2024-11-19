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
    public class UpdateEventCommandHandler : IUpdateEventCommandHandler
    {
        private readonly ILogger<UpdateEventCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        public UpdateEventCommandHandler(ILogger<UpdateEventCommandHandler> logger, IEventRepository eventRepository) 
        {
            _logger = logger;
            _eventRepository = eventRepository;
        }
        /// <summary>
        /// This method is responsible to handle updating of an event together with the attendees details.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> HandleAsync(Guid eventId, UpdateEventCommand command) 
        {
            try 
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(UpdateEventCommandHandler)} - {nameof(HandleAsync)}: attempting to update event details {eventId}.");

                var currentEvent = new Event
                {
                    Id = eventId,
                    Title = command.Title,
                    Description = command.Description,
                    StartTime = command.StartTime ?? DateTime.UtcNow.AddDays(2),
                    EndTime = command.EndTime ?? DateTime.UtcNow.AddDays(2).AddHours(1),
                    Attendees = command.Attendees
                };

                if( await _eventRepository.UpdateEventAsync(currentEvent)) 
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(UpdateEventCommandHandler)} - {nameof(HandleAsync)}: successfully updated event details {eventId}.");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(UpdateEventCommandHandler)} - {nameof(HandleAsync)}: failed to update an event {command.Title}.");
                    return false;
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(UpdateEventCommandHandler)} - {nameof(HandleAsync)}: failed to update an event {command.Title}. {ex.Message}.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
