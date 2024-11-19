using AutoMapper;
using DoctorSchedule.Application.Queries;
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
    public class GetEventsBetweenDatesQueryHandler : IGetEventsBetweenDatesQueryHandler
    {
        private readonly ILogger<GetEventsBetweenDatesQueryHandler> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public GetEventsBetweenDatesQueryHandler(ILogger<GetEventsBetweenDatesQueryHandler> logger, IEventRepository eventRepository, IMapper mapper)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<List<EventResponse>> HandleAsync(GetEventsBetweenDatesQuery query)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(GetEventByIdQueryHandler)} - {nameof(HandleAsync)}: attempting to get events between {query.StartTime} and {query.EndTime}.");

                var events = await _eventRepository.GetEventsAsync(query.StartTime, query.EndTime);

                if (events != null)
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(GetEventByIdQueryHandler)} - {nameof(HandleAsync)}: successfully retrieved events between {query.StartTime} and {query.EndTime}.");
                    return _mapper.Map<List<EventResponse>>(events);
                }
                else
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(GetEventByIdQueryHandler)} - {nameof(HandleAsync)}: no events between {query.StartTime} and {query.EndTime}.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetEventsBetweenDatesQueryHandler)} - {nameof(HandleAsync)}: failed to get an events.");
                throw new Exception($"{DateTime.Now}  - internal server error");
            }
        }
    }
}
