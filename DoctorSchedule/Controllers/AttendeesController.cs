using AutoMapper;
using DoctorSchedule.Authorization;
using DoctorSchedule.Domain.Entities;
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
        public AttendeesController(IEventRepository eventRepository,IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
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
            await _eventRepository.AcceptEventAsync(eventId, attendeeId);
            return Ok(new { Message = "Event accepted successfully." });
        }

        [HttpPost("{attendeeId}/decline")]
        public async Task<IActionResult> DeclineEvent(Guid eventId, Guid attendeeId)
        {
            await _eventRepository.DeclineEventAsync(eventId, attendeeId);
            return Ok(new { Message = "Event declined successfully." });
        }
    }
}
