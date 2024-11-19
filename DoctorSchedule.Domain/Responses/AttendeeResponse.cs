using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.Responses
{
    public class AttendeeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAttending { get; set; }
        public Guid EventId { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
