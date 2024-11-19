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
    public class UpdateAttendeeCommandHandler : IUpdateAttendeeCommandHandler
    {
        private readonly ILogger<UpdateAttendeeCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        public UpdateAttendeeCommandHandler(ILogger<UpdateAttendeeCommandHandler> logger, IEventRepository eventRepository ) 
        {
            _logger = logger;
            _eventRepository = eventRepository;
        }

        /// <summary>
        /// This method is responsible to handle the command to update attendee details.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="attendeeId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Attendee> HandleAsync(Guid eventId, Guid attendeeId, UpdateAttendeeCommad command)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(UpdateAttendeeCommandHandler)} - {nameof(HandleAsync)}: attempting to update attendee details {attendeeId}.");

                var attendee= new Attendee
                {
                    Id = attendeeId,
                    Name = command.Name,
                    Email = command.Email,
                    IsAttending = command.IsAttending ?? false
                };

                if (await _eventRepository.UpdateAttendeeDetailsAsync(eventId, attendee))
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(UpdateAttendeeCommandHandler)} - {nameof(HandleAsync)}: successfully updated attendee details {attendeeId}.");
                    return attendee;
                }
                else 
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(UpdateAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to updated attendee details {attendeeId}.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to add an attendee {command.Name}. {ex.Message}.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

    }
}
