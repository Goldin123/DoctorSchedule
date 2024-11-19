using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Commands;
using DoctorSchedule.Application.Queries;
using DoctorSchedule.Application.QueryHandlers.Interface;
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
        private readonly IGetEventByIdQueryHandler _getEventByIdQueryHandler;
        private readonly IGetEventsBetweenDatesQueryHandler _getEventsBetweenDatesQueryHandler;

        public EventsController(IEventRepository eventRepository, ICreateEventCommandHandler eventCommandHandler, IGetEventByIdQueryHandler getEventByIdQueryHandler, IGetEventsBetweenDatesQueryHandler getEventsBetweenDatesQueryHandler)
        {
            _eventRepository = eventRepository;
            _eventCommandHandler = eventCommandHandler;
            _getEventByIdQueryHandler = getEventByIdQueryHandler;
            _getEventsBetweenDatesQueryHandler = getEventsBetweenDatesQueryHandler;
        }

        [HttpPost("create-attendee-event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
        {
            var calendarEvent = await _eventCommandHandler.HandleAsync(command);

            if (calendarEvent != null)
                return CreatedAtAction(nameof(GetEventById), new { id = calendarEvent.Id }, calendarEvent);
            else
                return BadRequest();
        }

        [HttpGet("get-event-by-id/{id}")]
        public async Task<IActionResult> GetEventById(Guid eventId)
        {
            var calendarEvent = await _getEventByIdQueryHandler.HandleAsync(eventId);
            if (calendarEvent == null)
                return NotFound();
            return Ok(calendarEvent);
        }

        [HttpGet("get-events-by-dates")]
        public async Task<IActionResult> GetEvents([FromQuery] GetEventsBetweenDatesQuery query)
        {
            var events = await _getEventsBetweenDatesQueryHandler.HandleAsync(query);
            if (events != null)
                return Ok(events);
            else
                return NotFound();
        }
    }
}
