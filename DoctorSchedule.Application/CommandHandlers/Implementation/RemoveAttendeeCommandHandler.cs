using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Domain.RepositoriesInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.CommandHandlers.Implementation
{
    public class RemoveAttendeeCommandHandler : IRemoveAttendeeCommandHandler
    {
        private readonly ILogger<RemoveAttendeeCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        public RemoveAttendeeCommandHandler(ILogger<RemoveAttendeeCommandHandler> logger, IEventRepository eventRepository ) 
        { 
            _logger = logger;
            _eventRepository = eventRepository;
        }

        /// <summary>
        /// This method is responsible to handle the command to remove an attendee.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> HandleAsync(Guid eventId, Guid attendeeId) 
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(RemoveAttendeeCommandHandler)} - {nameof(HandleAsync)}: attempting to remove attendee {attendeeId}.");

                if(await _eventRepository.RemoveAttendeeAsync(eventId, attendeeId))
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(RemoveAttendeeCommandHandler)} - {nameof(HandleAsync)}: successfully to removed attendee {attendeeId}.");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(RemoveAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to remove attendee {attendeeId}.");
                    return false;
                }

            }
            catch (Exception ex) 
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(RemoveAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to remove attendee {attendeeId}. {ex.Message}.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
