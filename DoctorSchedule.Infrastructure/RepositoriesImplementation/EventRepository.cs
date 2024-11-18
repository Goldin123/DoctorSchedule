using AutoMapper;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Enums;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Domain.Responses;
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
        private readonly IMapper _mapper;

        public AppDbContext Context { get; }
        public ILogger<EventRepository> Logger { get; }

        public EventRepository(AppDbContext context, ILogger<EventRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public EventRepository(AppDbContext context, ILogger<EventRepository> logger)
        {
            Context = context;
            Logger = logger;
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

        public async Task<EventResponse> GetEventResponseByIdAsync(Guid eventId)
        {
            try
            {
                var currentEvent = await _context.Events
                    .Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                return _mapper.Map<EventResponse>(currentEvent);
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
                var existingEvent = await _context.Events.FirstOrDefaultAsync(x => x.Title == calendarEvent.Title);
                if (existingEvent == null)
                {
                    await _context.Events.AddAsync(calendarEvent);
                    await _context.SaveChangesAsync();
                }
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

                var existingAttendee = await _context.Attendees.FirstOrDefaultAsync(x => attendee.EventId == eventId && x.Email == attendee.Email);

                if (existingAttendee == null)
                {
                    attendee.Id = Guid.NewGuid();
                    attendee.EventId = eventId;
                    await _context.Attendees.AddAsync(attendee);
                    await _context.SaveChangesAsync();
                }
                //else
                //    await UpdateAttendeeAsync(eventId, attendee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task<bool> UpdateAttendeeAsync(Guid eventId, Attendee updatedAttendee)
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
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task<bool> RemoveAttendeeAsync(Guid eventId, Guid attendeeId)
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
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task<bool> AcceptEventAsync(Guid eventId, Guid attendeeId)
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
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }

        public async Task<bool> DeclineEventAsync(Guid eventId, Guid attendeeId)
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
                attendee.IsAttending = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}  - internal server error - {ex.Message}");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
