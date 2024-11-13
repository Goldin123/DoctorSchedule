using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Domain.Requests
{
    public class UpdateAttendeeRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAttending { get; set; }
    }
}
