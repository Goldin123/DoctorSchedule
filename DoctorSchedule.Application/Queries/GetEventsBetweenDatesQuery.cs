using DoctorSchedule.Application.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.Queries
{
    public class GetEventsBetweenDatesQuery
    {
        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }
        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.DateTime)]
        [TimeComparison("StartTime", "EndTime", ErrorMessage = "End time must be later than start time.")]
        public DateTime? EndTime { get; set; }
    }
}
