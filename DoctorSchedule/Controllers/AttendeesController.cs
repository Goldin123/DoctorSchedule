using AutoMapper;
using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Authorization;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Models;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Domain.Requests;
using DoctorSchedule.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AttendeesController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IMessageQueue _messageQueue;
        public AttendeesController(IEventRepository eventRepository,IMapper mapper, IMessageQueue messageQueue)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _messageQueue = messageQueue;   
        }

        [HttpPost("add-attendee")]
        public async Task<IActionResult> AddAttendee(Guid eventId, [FromBody] AddAttendeeRequest request)
        {
            var attendee = new Attendee
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                IsAttending = request.IsAttending
            };

            await _eventRepository.AddAttendeeAsync(eventId, attendee);
            return CreatedAtAction(nameof(GetAttendee), new { eventId, attendeeId = attendee.Id }, attendee);
        }

        [HttpGet("get-attendee-by-id/{attendeeId}")]
        public async Task<IActionResult> GetAttendee(Guid eventId, Guid attendeeId)
        {
            var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);
            if (calendarEvent == null)
            {
                return NotFound("Event not found.");
            }

            var attendee = _mapper.Map<AttendeeResponse>(calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId));
            if (attendee == null)
            {
                return NotFound("Attendee not found.");
            }

            return Ok(attendee);
        }

        [HttpPut("update-attendee-details/{attendeeId}")]
        public async Task<IActionResult> UpdateAttendee(Guid eventId, Guid attendeeId, [FromBody] UpdateAttendeeRequest request)
        {
            var updatedAttendee = new Attendee
            {
                Id = attendeeId,
                Name = request.Name,
                Email = request.Email,
                IsAttending = request.IsAttending
            };

            await _eventRepository.UpdateAttendeeAsync(eventId, updatedAttendee);
            return NoContent();
        }

        [HttpDelete("delete-attendee/{attendeeId}")]
        public async Task<IActionResult> RemoveAttendee(Guid eventId, Guid attendeeId)
        {
            await _eventRepository.RemoveAttendeeAsync(eventId, attendeeId);
            return NoContent();
        }

        [HttpPost("{attendeeId}/accept")]
        public async Task<IActionResult> AcceptEvent(Guid eventId, Guid attendeeId)
        {
            var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);
            if (calendarEvent == null)
                return NotFound("Event not found.");

            var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);
            
            if (attendee == null) return NotFound("Attendee not found.");
           
            await _eventRepository.AcceptEventAsync(eventId, attendeeId);
            await _messageQueue.SendAsync(new NotificationMessage
            {
                Email = attendee.Email,
                Message = $"Event {calendarEvent.Title} for {attendee.Name} accepted please note it is scheduled on {calendarEvent.StartTime} ending on {calendarEvent.EndTime}."
            });

            return Ok(new { Message = $"Event for {attendee.Name} accepted successfully and notification sent." });
        }

        [HttpPost("{attendeeId}/decline")]
        public async Task<IActionResult> DeclineEvent(Guid eventId, Guid attendeeId)
        {
            var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);
            if (calendarEvent == null)
                return NotFound("Event not found.");

            var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);

            if (attendee == null) return NotFound("Attendee not found.");

            await _eventRepository.DeclineEventAsync(eventId, attendeeId);
            await _messageQueue.SendAsync(new NotificationMessage
            {
                Email = attendee.Email,
                Message = $"Event {calendarEvent.Title} for {attendee.Name} is declined."
            });
            return Ok(new { Message = $"Event for {attendee.Name} declined successfully and notification sent." });
        }
    }
}
