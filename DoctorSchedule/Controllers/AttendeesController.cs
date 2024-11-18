using AutoMapper;
using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Commands;
using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Authorization;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Models;
using DoctorSchedule.Domain.RepositoriesInterface;
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
        private readonly ICreateAttendeeCommandHandler _createAttendeeCommandHandler;
        private readonly IUpdateAttendeeCommadHandler _updateAttendeeCommadHandler;
        public AttendeesController(IEventRepository eventRepository, IMapper mapper, IMessageQueue messageQueue, ICreateAttendeeCommandHandler createAttendeeCommandHandler, IUpdateAttendeeCommadHandler updateAttendeeCommadHandler)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _messageQueue = messageQueue;
            _createAttendeeCommandHandler = createAttendeeCommandHandler;
            _updateAttendeeCommadHandler = updateAttendeeCommadHandler;
        }

        [HttpPost("add-attendee")]
        public async Task<IActionResult> AddAttendee(Guid eventId, [FromBody] CreateAttendeeCommand command)
        {
            var attendee = await _createAttendeeCommandHandler.Handle(eventId, command);

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
        public async Task<IActionResult> UpdateAttendee(Guid eventId, Guid attendeeId, [FromBody] UpdateAttendeeCommad commad)
        {
            var updatedAttendee = await _updateAttendeeCommadHandler.Handle(eventId, attendeeId, commad);

            if (await _eventRepository.UpdateAttendeeAsync(eventId, updatedAttendee))
                return Ok("Successfully updated.");
            return NoContent();
        }

        [HttpDelete("delete-attendee/{attendeeId}")]
        public async Task<IActionResult> RemoveAttendee(Guid eventId, Guid attendeeId)
        {
            if (await _eventRepository.RemoveAttendeeAsync(eventId, attendeeId))
                return Ok("Successfully removed.");
            return NoContent();
        }

        [HttpPost("{attendeeId}/accept")]
        public async Task<IActionResult> AcceptEvent(Guid eventId, Guid attendeeId)
        {
            var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);

            if (calendarEvent == null) return NotFound("Event not found.");

            var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);

            if (attendee == null) return NotFound("Attendee not found.");

            if (await _eventRepository.AcceptEventAsync(eventId, attendeeId))
            {

                await _messageQueue.SendAsync(new NotificationMessage
                {
                    Email = attendee.Email,
                    Message = $"Event {calendarEvent.Title} for {attendee.Name} accepted please note it is scheduled on {calendarEvent.StartTime} ending on {calendarEvent.EndTime}."
                });

                return Ok(new { Message = $"Event for {attendee.Name} accepted successfully and notification sent." });
            }
            return NoContent();
        }

        [HttpPost("{attendeeId}/decline")]
        public async Task<IActionResult> DeclineEvent(Guid eventId, Guid attendeeId)
        {
            var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);

            if (calendarEvent == null) return NotFound("Event not found.");

            var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);

            if (attendee == null) return NotFound("Attendee not found.");

            if (await _eventRepository.DeclineEventAsync(eventId, attendeeId))
            {

                await _messageQueue.SendAsync(new NotificationMessage
                {
                    Email = attendee.Email,
                    Message = $"Event {calendarEvent.Title} for {attendee.Name} is declined."
                });
                return Ok(new { Message = $"Event for {attendee.Name} declined successfully and notification sent." });
            }
            return NoContent();
        }
    }
}
