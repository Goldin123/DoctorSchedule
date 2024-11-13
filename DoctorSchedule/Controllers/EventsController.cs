using DoctorSchedule.Application.Commands;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var calendarEvent = await _eventRepository.GetEventByIdAsync(id);
            if (calendarEvent == null)
            {
                return NotFound();
            }
            return Ok(calendarEvent);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
        {
            var calendarEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                Description = command.Description,
                StartTime = command.StartTime,
                EndTime = command.EndTime,
                Attendees = command.Attendees
            };
            await _eventRepository.CreateEventAsync(calendarEvent);
            return CreatedAtAction(nameof(GetEventById), new { id = calendarEvent.Id }, calendarEvent);
        }
    }
}
