using AutoMapper;
using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Application.Commands;
using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Application.QueryHandlers.Interface;
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
        private readonly ICreateAttendeeCommandHandler _createAttendeeCommandHandler;
        private readonly IUpdateAttendeeCommandHandler _updateAttendeeCommadHandler;
        private readonly IGetAttendeeQueryHandler _getAttendeeQueryHandler;
        private readonly IRemoveAttendeeCommandHandler _removeAttendeeCommandHandler;
        private readonly IAcceptEventCommandHandler _acceptEventCommandHandler;
        private readonly IDeclineEventCommandHandler _declineEventCommandHandler;
        public AttendeesController(ICreateAttendeeCommandHandler createAttendeeCommandHandler, IUpdateAttendeeCommandHandler updateAttendeeCommadHandler, IGetAttendeeQueryHandler getAttendeeQueryHandler, IRemoveAttendeeCommandHandler removeAttendeeCommandHandler, IAcceptEventCommandHandler acceptEventCommandHandler, IDeclineEventCommandHandler declineEventCommandHandler)
        {
            _createAttendeeCommandHandler = createAttendeeCommandHandler;
            _updateAttendeeCommadHandler = updateAttendeeCommadHandler;
            _getAttendeeQueryHandler = getAttendeeQueryHandler;
            _removeAttendeeCommandHandler = removeAttendeeCommandHandler;
            _acceptEventCommandHandler = acceptEventCommandHandler;
            _declineEventCommandHandler = declineEventCommandHandler;
        }

        [HttpPost("add-attendee")]
        public async Task<IActionResult> AddAttendee(Guid eventId, [FromBody] CreateAttendeeCommand command)
        {
            var attendee = await _createAttendeeCommandHandler.HandleAsync(eventId, command);
            if (attendee != null)
                return CreatedAtAction(nameof(GetAttendee), new { eventId, attendeeId = attendee.Id }, attendee);
            else
                return BadRequest();
        }

        [HttpGet("get-attendee-by-id/{attendeeId}")]
        public async Task<IActionResult> GetAttendee(Guid eventId, Guid attendeeId)
        {
            var attendee = await _getAttendeeQueryHandler.HandleAsync(eventId, attendeeId);
            if (attendee != null)
                return Ok(attendee);
            else
                return NotFound("Attendee or Event not found.");
        }

        [HttpPut("update-attendee-details/{attendeeId}")]
        public async Task<IActionResult> UpdateAttendee(Guid eventId, Guid attendeeId, [FromBody] UpdateAttendeeCommad commad)
        {
            var updatedAttendee = await _updateAttendeeCommadHandler.HandleAsync(eventId, attendeeId, commad);

            if (updatedAttendee != null)
                return Ok("Attendee successfully updated.");
            else 
                return BadRequest();
        }

        [HttpDelete("delete-attendee/{attendeeId}")]
        public async Task<IActionResult> RemoveAttendee(Guid eventId, Guid attendeeId)
        {
            if (await _removeAttendeeCommandHandler.HandleAsync(eventId, attendeeId))
                return Ok("Attendee successfully removed.");
            else
                return BadRequest();
        }

        [HttpPost("{attendeeId}/accept")]
        public async Task<IActionResult> AcceptEvent(Guid eventId, Guid attendeeId)
        {
            if (await _acceptEventCommandHandler.HandleAsync(eventId, attendeeId))
                return Ok(new { Message = $"Event accepted successfully and notification sent." });
            else
                return BadRequest();
        }

        [HttpPost("{attendeeId}/decline")]
        public async Task<IActionResult> DeclineEvent(Guid eventId, Guid attendeeId)
        {
            if (await _declineEventCommandHandler.HandleAsync(eventId, attendeeId))
                return Ok(new { Message = $"Event decline successfully and notification sent." });
            else
                return BadRequest();
        }
    }
}
