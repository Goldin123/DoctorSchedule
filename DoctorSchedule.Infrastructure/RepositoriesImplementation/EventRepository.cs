using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Infrastructure.RepositoriesImplementation
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            return await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<List<Event>> GetEventsAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Events.Include(e => e.Attendees).AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.StartTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.EndTime <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task CreateEventAsync(Event calendarEvent)
        {
            await _context.Events.AddAsync(calendarEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event calendarEvent)
        {
            _context.Events.Update(calendarEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(Guid eventId)
        {
            var calendarEvent = await _context.Events.FindAsync(eventId);
            if (calendarEvent != null)
            {
                _context.Events.Remove(calendarEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
