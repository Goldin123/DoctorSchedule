using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.Entities
{
    /// <summary>
    /// This will represent the patient who is visiting the doctor.
    /// </summary>
    public class Attendee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAttending { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}
