using AutoMapper;
using DoctorSchedule.Application.QueryHandlers.Interface;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.RepositoriesInterface;
using DoctorSchedule.Domain.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.QueryHandlers.Implementation
{
    public class GetEventByIdQueryHandler : IGetEventByIdQueryHandler
    {
        private readonly ILogger<GetEventByIdQueryHandler> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public GetEventByIdQueryHandler(ILogger<GetEventByIdQueryHandler> logger, IEventRepository eventRepository, IMapper mapper)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<EventResponse> HandleAsync(Guid id) 
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(GetEventByIdQueryHandler)} - {nameof(HandleAsync)}: attempting to get an event {id}.");
                var calendarEvent = await _eventRepository.GetEventResponseByIdAsync(id);
               
                if (calendarEvent != null) 
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(GetEventByIdQueryHandler)} - {nameof(HandleAsync)}: successfully found an event {id}.");
                    return calendarEvent;
                }
                else 
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(GetEventByIdQueryHandler)} - {nameof(HandleAsync)}: failed to found an event {id}.");
                    return null;
                }

            }
            catch (Exception ex) 
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetEventByIdQueryHandler)} - {nameof(HandleAsync)}: failed to get an event.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        } 
    }
}
