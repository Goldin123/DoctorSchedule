using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Domain.Entities;
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
    public class AcceptEventCommandHandler : IAcceptEventCommandHandler
    {
        private readonly ILogger<AcceptEventCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IMessageQueue _messageQueue;

        public AcceptEventCommandHandler(ILogger<AcceptEventCommandHandler> logger, IEventRepository eventRepository, IMessageQueue messageQueue)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _messageQueue = messageQueue;
        }

        /// <summary>
        /// This method is responsible accept attendees to events.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> HandleAsync(Guid eventId, Guid attendeeId)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(AcceptEventCommandHandler)} - {nameof(HandleAsync)}: attempting to accept an attendee {attendeeId} to event {eventId}.");

                var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);

                if (calendarEvent == null)
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(AcceptEventCommandHandler)} - {nameof(HandleAsync)}: no found event {eventId}.");
                    throw new KeyNotFoundException("Event not found.");
                }

                var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);

                if (attendee == null)
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(AcceptEventCommandHandler)} - {nameof(HandleAsync)}: no found attendee {attendeeId}.");
                    throw new KeyNotFoundException("Attendee not found.");
                }

                if (await _eventRepository.ResponseStatusEventAsync(eventId, attendeeId,ResponseStatus.Accepted,true))
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(AcceptEventCommandHandler)} - {nameof(HandleAsync)}: successfully accepted an attendee {attendeeId} to event {eventId}. About to send a notification.");

                    await _messageQueue.SendAsync(new NotificationMessage
                    {
                        Email = attendee.Email,
                        Message = $"Event {calendarEvent.Title} for {attendee.Name} accepted please note it is scheduled on {calendarEvent.StartTime} ending on {calendarEvent.EndTime}."
                    });
                    return true;
                }
                else 
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(AcceptEventCommandHandler)} - {nameof(HandleAsync)}: failed to accept event {eventId} for attendee {attendeeId}.");
                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(AcceptEventCommandHandler)} - {nameof(HandleAsync)}: failed to accept event {eventId} for attendee {attendeeId}. {ex.Message}.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
