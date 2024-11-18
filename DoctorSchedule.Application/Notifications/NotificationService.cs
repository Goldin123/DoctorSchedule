using DoctorSchedule.Application.Messaging.Interface;
using DoctorSchedule.Domain.Entities;
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

        public async Task NotifyAttendeesAsync(Event calendarEvent)
        {
            try
            {
                foreach (var attendee in calendarEvent.Attendees)
                {
                    if (attendee.IsAttending)
                    {
                        await _messageQueue.SendAsync(new NotificationMessage
                        {
                            Email = attendee.Email,
                            Message = $"Event {calendarEvent.Title} scheduled on {calendarEvent.StartTime}"
                        });
                    }
                }
            }
            catch (Exception ex) 
            {
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
