using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Domain.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public AttendeesController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpPost]
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

        [HttpGet("{attendeeId}")]
        public async Task<IActionResult> GetAttendee(Guid eventId, Guid attendeeId)
        {
            var calendarEvent = await _eventRepository.GetEventByIdAsync(eventId);
            if (calendarEvent == null)
            {
                return NotFound("Event not found.");
            }

            var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);
            if (attendee == null)
            {
                return NotFound("Attendee not found.");
            }

            return Ok(attendee);
        }

        [HttpPut("{attendeeId}")]
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

        [HttpDelete("{attendeeId}")]
        public async Task<IActionResult> RemoveAttendee(Guid eventId, Guid attendeeId)
        {
            await _eventRepository.RemoveAttendeeAsync(eventId, attendeeId);
            return NoContent();
        }
    }
}
