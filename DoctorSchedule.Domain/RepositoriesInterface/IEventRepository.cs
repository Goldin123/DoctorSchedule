using DoctorSchedule.Domain.Aggregates;
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
        Task CreateEventAsync(Event calendarEvent);
        Task UpdateEventAsync(Event calendarEvent);
        Task DeleteEventAsync(Guid eventId);
    }
}

