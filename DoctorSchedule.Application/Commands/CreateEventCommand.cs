using DoctorSchedule.Application.Attributes;
using DoctorSchedule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.Commands
{
    public class CreateEventCommand
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(5000, ErrorMessage = "Description cannot exceed 5000 characters.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Start time is required.")]
        [GreaterThanCurrentTime(ErrorMessage = "Start time must be in the future.")]
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }
        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.DateTime)]
        [TimeComparison("StartTime", "EndTime", ErrorMessage = "End time must be later than start time.")]
        public DateTime? EndTime { get; set; }
        public List<Attendee>? Attendees { get; set; }
    }
}

