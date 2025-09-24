using CavistaEventCelebration.Api.Dto;
using CavistaEventCelebration.Api.Dto.EmployeeEvent;
using CavistaEventCelebration.Api.Dto.Event;
using CavistaEventCelebration.Api.Models;

namespace CavistaEventCelebration.Api.Services.Interface
{
    public interface IEventService
    {
        Task<Response<bool>> AddEvent(EventDto newEvent);
        Task<Response<bool>> DeleteEvent(int id);
        Task<Response<List<Event>>> Events();
        Task<Response<bool>> UpdateEvent(int id, EventDto updateEvent);
        Task<Response<bool>> AddEmployeeEvent(AddEmployeeEventDto employeeEvent);
        Task<Response<bool>> DeleteEmployeeEvent(Guid id);
        Task<PaginatedList<EmployeeEventDto>> EmployeeEvents(int? index, int? pageSize, string? searchString);
        Task<Response<bool>> UpdateEployeeEvent(UpdateEmployeeEventDto employeeEvent);
    }
}
