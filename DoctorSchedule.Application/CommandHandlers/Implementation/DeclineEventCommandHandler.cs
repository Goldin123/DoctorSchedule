using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Domain.Enums;
using DoctorSchedule.Domain.Models;
using DoctorSchedule.Domain.RepositoriesInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.CommandHandlers.Implementation
{
    public class DeclineEventCommandHandler : IDeclineEventCommandHandler
    {
        private readonly ILogger<DeclineEventCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IMessageQueue _messageQueue;
        public DeclineEventCommandHandler(ILogger<DeclineEventCommandHandler> logger, IEventRepository eventRepository, IMessageQueue messageQueue)
        {
            _eventRepository = eventRepository;
            _messageQueue = messageQueue;
            _logger = logger;
        }
        /// <summary>
        /// This method is responsible to handle commands related to declining an event for an attendee.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public async Task<bool> HandleAsync(Guid eventId, Guid attendeeId)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(DeclineEventCommandHandler)} - {nameof(HandleAsync)}: attempting to decline an attendee {attendeeId} to event {eventId}.");

                var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);

                if (calendarEvent == null)
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(DeclineEventCommandHandler)} - {nameof(HandleAsync)}: no found event {eventId}.");
                    throw new KeyNotFoundException("Event not found.");
                }

                var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);

                if (attendee == null)
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(DeclineEventCommandHandler)} - {nameof(HandleAsync)}: no found attendee {attendeeId}.");
                    throw new KeyNotFoundException("Attendee not found.");
                }

                if (await _eventRepository.ResponseStatusEventAsync(eventId, attendeeId, ResponseStatus.Declined, false))
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(DeclineEventCommandHandler)} - {nameof(HandleAsync)}: successfully decline an attendee {attendeeId} to event {eventId}. About to send a notification.");

                    await _messageQueue.SendAsync(new NotificationMessage
                    {
                        Email = attendee.Email,
                        Message = $"Event {calendarEvent.Title} for {attendee.Name} was declined."
                    });
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(DeclineEventCommandHandler)} - {nameof(HandleAsync)}: failed to decline event {eventId} for attendee {attendeeId}.");

                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(DeclineEventCommandHandler)} - {nameof(HandleAsync)}: failed to decline event {eventId} for attendee {attendeeId}. {ex.Message}.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
