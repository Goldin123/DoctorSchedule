using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Enums;
using DoctorSchedule.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.RepositoriesInterface
{
    public interface IEventRepository
    {
        Task<Event> GetEventByIdAsync(Guid eventId);
        Task<List<Event>> GetEventsAsync(DateTime? startDate, DateTime? endDate);
        Task<bool> CreateEventAsync(Event calendarEvent);
        Task<bool> UpdateEventAsync(Event calendarEvent);
        Task<bool> DeleteEventAsync(Guid eventId);
        Task<bool> AddAttendeeAsync(Guid eventId, Attendee attendee);
        Task<bool> UpdateAttendeeDetailsAsync(Guid eventId, Attendee attendee);
        Task<bool> RemoveAttendeeAsync(Guid eventId, Guid attendeeId);
        Task<EventResponse> GetEventResponseByIdAsync(Guid eventId);
        Task<bool> ResponseStatusEventAsync(Guid eventId, Guid attendeeId, ResponseStatus responseStatus, bool isAttending);
    }
}
