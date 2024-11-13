using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Domain.Aggregates;
using DoctorSchedule.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.Notifications
{
    public class NotificationService
    {
        private readonly IMessageQueue _messageQueue;

        public NotificationService(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public void NotifyAttendees(Event calendarEvent)
        {
            foreach (var attendee in calendarEvent.Attendees)
            {
                if (attendee.IsAttending)
                {
                    _messageQueue.Send(new NotificationMessage
                    {
                        Email = attendee.Email,
                        Message = $"Event {calendarEvent.Title} scheduled on {calendarEvent.StartTime}"
                    });
                }
            }
        }
    }
}
