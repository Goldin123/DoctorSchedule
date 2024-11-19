using AutoMapper;
using DoctorSchedule.Application.Commands;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Responses;

namespace DoctorSchedule.Middleware
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<Event, EventResponse>();
            CreateMap<Attendee, AttendeeResponse>();

        }
    }
}
