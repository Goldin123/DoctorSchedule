using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.Messaging.Implementation
{
    public class InMemoryMessageQueue : IMessageQueue
    {
        private readonly ILogger<InMemoryMessageQueue> _logger;

        public InMemoryMessageQueue(ILogger<InMemoryMessageQueue> logger)
        {
            _logger = logger;
        }

        public void Send(NotificationMessage message)
        {
            // Log the message instead of actually sending it
            _logger.LogInformation($"Message sent to {message.Email}: {message.Message}");
        }
    }
}
