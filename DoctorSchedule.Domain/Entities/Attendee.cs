using DoctorSchedule.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.Entities
{
    /// <summary>
    /// This will represent the patient who is visiting the doctor.
    /// </summary>
    public class Attendee
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAttending { get; set; }
        [JsonIgnore]
        public Guid EventId { get; set; }       
        public ResponseStatus ResponseStatus { get; set; } = ResponseStatus.Pending;
    }
}
