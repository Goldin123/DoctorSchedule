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
    public class CreateAttendeeCommandHandler : ICreateAttendeeCommandHandler
    {
        private readonly ILogger<CreateAttendeeCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        public CreateAttendeeCommandHandler(ILogger<CreateAttendeeCommandHandler> logger, IEventRepository  eventRepository) 
        {
            _logger = logger;
            _eventRepository = eventRepository;
        }
        /// <summary>
        /// This method is responsible to handle the create attendee command.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Attendee> HandleAsync(Guid eventId, CreateAttendeeCommand command)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: attempting to add an attendee {command.Name}.");

                var attendee =  new Attendee
                {
                    Id = Guid.NewGuid(),
                    Name = command.Name,
                    Email = command.Email,
                    IsAttending = command.IsAttending ?? false,
                    EventId = eventId
                };

                if (await _eventRepository.AddAttendeeAsync(eventId, attendee))
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: successfully added an attendee {command.Name}.");
                else
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to add an attendee {command.Name}.");
                    return null;
                }

                return attendee;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(CreateAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to add an attendee {command.Name}. {ex.Message}.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
