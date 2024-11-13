using DoctorSchedule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.Aggregates
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Attendee> Attendees { get; set; } = new();

        public void AddAttendee(Attendee attendee)
        {
            if (!Attendees.Any(a => a.Email == attendee.Email))
                Attendees.Add(attendee);
        }

        public void UpdateEvent(string title, string description, DateTime startTime, DateTime endTime)
        {
            Title = title;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
