using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Commands;
using DoctorSchedule.Authorization;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICreateEventCommandHandler _eventCommandHandler;

        public EventsController(IEventRepository eventRepository, ICreateEventCommandHandler eventCommandHandler)
        {
            _eventRepository = eventRepository;
            _eventCommandHandler = eventCommandHandler;
        }

        [HttpGet("get-event-by-id/{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var calendarEvent = await _eventRepository.GetEventResponseByIdAsync(id);
            if (calendarEvent == null)
            {
                return NotFound();
            }
            return Ok(calendarEvent);
        }

        [HttpGet("get-events-by-dates")]
        public async Task<IActionResult> GetEvents([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return BadRequest("Both startDate and endDate are required.");
            }

            var events = await _eventRepository.GetEventsAsync(startDate, endDate);
            return Ok(events);
        }

        [HttpPost("create-attendee-event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
            {
            var calendarEvent = await _eventCommandHandler.Handle(command);
            await _eventRepository.CreateEventAsync(calendarEvent);
            return CreatedAtAction(nameof(GetEventById), new { id = calendarEvent.Id }, calendarEvent);
        }
    }
}
