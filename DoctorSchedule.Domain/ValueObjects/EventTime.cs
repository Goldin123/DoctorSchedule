using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.ValueObjects
{
    public class EventTime
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public EventTime(DateTime startTime, DateTime endTime)
        {
            if (endTime <= startTime)
                throw new ArgumentException("End time must be after start time.");

            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
