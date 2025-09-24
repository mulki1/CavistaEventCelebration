using CavistaEventCelebration.Api.Dto.EmployeeEvent;
using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using CavistaEventCelebration.Api.Dto;
using Microsoft.AspNetCore.Authorization;

namespace CavistaEventCelebration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeEventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EmployeeEventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeEvent(AddEmployeeEventDto employeeEvent)
        {
            return Ok(await _eventService.AddEmployeeEvent(employeeEvent));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployeeEvent(UpdateEmployeeEventDto employeeEvent)
        {
            return Ok(await _eventService.UpdateEployeeEvent(employeeEvent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeevent(Guid id)
        {
            return Ok(await _eventService.DeleteEmployeeEvent(id));
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<EmployeeEventDto>>> Get(int? index, int? pageSize, string? searchString)
        {
            return Ok(await _eventService.EmployeeEvents(index, pageSize, searchString));
        }
    }
}
