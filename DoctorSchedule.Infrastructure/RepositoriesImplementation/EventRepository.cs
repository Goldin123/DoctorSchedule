using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Enums;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EventRepository> _logger;
        public EventRepository(AppDbContext context, ILogger<EventRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            try
            {
                return await _context.Events
                    .Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task<List<Event>> GetEventsAsync(DateTime? startDate, DateTime? endDate)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task CreateEventAsync(Event calendarEvent)
        {
            try
            {
                await _context.Events.AddAsync(calendarEvent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task UpdateEventAsync(Event calendarEvent)
        {
            try
            {
                _context.Events.Update(calendarEvent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task DeleteEventAsync(Guid eventId)
        {
            try
            {
                var calendarEvent = await _context.Events.FindAsync(eventId);
                if (calendarEvent != null)
                {
                    _context.Events.Remove(calendarEvent);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task AddAttendeeAsync(Guid eventId, Attendee attendee)
        {
            try
            {
                var calendarEvent = await _context.Events.Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                if (calendarEvent == null) throw new KeyNotFoundException("Event not found.");

                calendarEvent.Attendees.Add(attendee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task UpdateAttendeeAsync(Guid eventId, Attendee updatedAttendee)
        {
            try
            {
                var calendarEvent = await _context.Events.Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                if (calendarEvent == null) throw new KeyNotFoundException("Event not found.");

                var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == updatedAttendee.Id);
                if (attendee == null) throw new KeyNotFoundException("Attendee not found.");

                attendee.Name = updatedAttendee.Name;
                attendee.Email = updatedAttendee.Email;
                attendee.IsAttending = updatedAttendee.IsAttending;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task RemoveAttendeeAsync(Guid eventId, Guid attendeeId)
        {
            try
            {
                var calendarEvent = await _context.Events.Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                if (calendarEvent == null) throw new KeyNotFoundException("Event not found.");

                var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);
                if (attendee != null)
                {
                    calendarEvent.Attendees.Remove(attendee);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task AcceptEventAsync(Guid eventId, Guid attendeeId)
        {
            try
            {
                var calendarEvent = await _context.Events
                    .Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                if (calendarEvent == null) throw new KeyNotFoundException("Event not found.");

                var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);
                if (attendee == null) throw new KeyNotFoundException("Attendee not found.");

                attendee.ResponseStatus = ResponseStatus.Accepted;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task DeclineEventAsync(Guid eventId, Guid attendeeId)
        {
            try
            {
                var calendarEvent = await _context.Events
                    .Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                if (calendarEvent == null) throw new KeyNotFoundException("Event not found.");

                var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);
                if (attendee == null) throw new KeyNotFoundException("Attendee not found.");

                attendee.ResponseStatus = ResponseStatus.Declined;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
