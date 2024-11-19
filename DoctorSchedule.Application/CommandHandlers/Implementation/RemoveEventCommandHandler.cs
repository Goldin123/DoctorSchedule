using DoctorSchedule.Application.CommandHandlers.Interface;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.CommandHandlers.Implementation
{
    public class RemoveEventCommandHandler : IRemoveEventCommandHandler
    {
        private readonly ILogger<RemoveEventCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        public RemoveEventCommandHandler(ILogger<RemoveEventCommandHandler> logger, IEventRepository eventRepository)
        {
            _logger = logger;
            _eventRepository = eventRepository;
        }

        public async Task<bool> HandleAsync(Guid eventId)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(RemoveEventCommandHandler)} - {nameof(HandleAsync)}: attempting to remove event {eventId}.");
                
                if (await _eventRepository.DeleteEventAsync(eventId))
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(RemoveEventCommandHandler)} - {nameof(HandleAsync)}: successfully removed event {eventId}.");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(RemoveAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to remove event {eventId}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(RemoveAttendeeCommandHandler)} - {nameof(HandleAsync)}: failed to remove event {eventId}. {ex.Message}.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
