using DoctorSchedule.Application.CommandHandlers.Implementation;
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
        private readonly ICreateEventCommandHandler _eventCommandHandler;
        private readonly IGetEventByIdQueryHandler _getEventByIdQueryHandler;
        private readonly IGetEventsBetweenDatesQueryHandler _getEventsBetweenDatesQueryHandler;
        private readonly IUpdateEventCommandHandler _updateEventCommandHandler;

        public EventsController(ICreateEventCommandHandler eventCommandHandler, IGetEventByIdQueryHandler getEventByIdQueryHandler, IGetEventsBetweenDatesQueryHandler getEventsBetweenDatesQueryHandler, IUpdateEventCommandHandler updateEventCommandHandler)
        {
            _eventCommandHandler = eventCommandHandler;
            _getEventByIdQueryHandler = getEventByIdQueryHandler;
            _getEventsBetweenDatesQueryHandler = getEventsBetweenDatesQueryHandler;
            _updateEventCommandHandler = updateEventCommandHandler;
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

        [HttpPut("update-event-details/{eventId}")]
        public async Task<IActionResult> UpdateEvent(Guid eventId, [FromBody] UpdateEventCommand command)
        {
            if(await _updateEventCommandHandler.HandleAsync(eventId, command))
                return Ok("Event successfully updated.");
            else
                return BadRequest();
        }

        [HttpGet("get-event-by-id/{eventId}")]
        public async Task<IActionResult> GetEventById(Guid eventId)
        {
            var calendarEvent = await _getEventByIdQueryHandler.HandleAsync(eventId);
            if (calendarEvent != null)
                return Ok(calendarEvent);
            else
                return NotFound("No event found.");
        }

        [HttpGet("get-events-by-dates")]
        public async Task<IActionResult> GetEvents([FromQuery] GetEventsBetweenDatesQuery query)
        {
            var events = await _getEventsBetweenDatesQueryHandler.HandleAsync(query);
            if (events != null)
                return Ok(events);
            else
                return NotFound("No events found.");
        }
    }
}
