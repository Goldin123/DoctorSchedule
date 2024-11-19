using DoctorSchedule.Application.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.CommandHandlers.Interface
{
    public interface IUpdateEventCommandHandler
    {
        Task<bool> HandleAsync(Guid eventId, UpdateEventCommand command);
    }
}
