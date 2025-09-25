﻿using CavistaEventCelebration.Api.Dto;
using CavistaEventCelebration.Api.Dto.Event;
using CavistaEventCelebration.Api.Models;

namespace CavistaEventCelebration.Api.Repositories.Interface
{
    public interface IEventRepo
    {
        Task<List<DailyEventDto>> GetDailyEvents(int eventId);
        Task<List<Event>> Events();
        Task<bool> AddEvent(Event eventItem);
        Task<bool> Remove(Event eventItem);
        Task<Event> GetById(int Id);
        Task<bool> UpdateEvent(Event eventItem);
        Task<bool> DoesEventExist(string Name);
        Task<bool> AddEmployeeEvent(EmployeeEvent eventItem);
        Task<bool> UpdateEmployeeEvent(EmployeeEvent eventItem);
        IQueryable<EmployeeEventDto> EmployeeEventGet(Guid currentUserId);
        Task<EmployeeEvent> GetEmployeeEventById(Guid id);
        Task<bool> DoesEmployeeEventExist(Guid employeeId, int eventId);
        Task<List<EmployeeEventDto>> GetEventsInRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<bool> UpdateEmployeeEvents(List<EmployeeEvent> employeeEvents);
        Task<List<EmployeeEvent>> GetEmployeeEvents(List<Guid> employeeEventIds);
        Task<ApplicationUser> user(Guid userId);
    }
}
