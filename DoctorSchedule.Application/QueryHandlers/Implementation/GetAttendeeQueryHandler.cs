using AutoMapper;
using DoctorSchedule.Application.CommandHandlers.Implementation;
using DoctorSchedule.Application.QueryHandlers.Interface;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Domain.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.QueryHandlers.Implementation
{
    public class GetAttendeeQueryHandler : IGetAttendeeQueryHandler
    {
        private readonly ILogger<GetAttendeeQueryHandler> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public GetAttendeeQueryHandler(ILogger<GetAttendeeQueryHandler> logger, IEventRepository eventRepository, IMapper mapper) 
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// This method is responsible to handle the retrieval of attendee details from the database.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<AttendeeResponse> HandleAsync(Guid eventId, Guid attendeeId)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(GetAttendeeQueryHandler)} - {nameof(HandleAsync)}: attempting to get an event {eventId}.");
                var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);

                if (calendarEvent == null)
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(GetAttendeeQueryHandler)} - {nameof(HandleAsync)}: event not found.");
                    return null;
                }

                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(GetAttendeeQueryHandler)} - {nameof(HandleAsync)}: attempting to get an attendee {attendeeId}.");
                
                var attendee = _mapper.Map<AttendeeResponse>(calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId));

                if (attendee == null) 
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(GetAttendeeQueryHandler)} - {nameof(HandleAsync)}: attendee not found.");
                    return null;
                }

                return attendee;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetAttendeeQueryHandler)} - {nameof(HandleAsync)}: failed to get an attendee or event.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
