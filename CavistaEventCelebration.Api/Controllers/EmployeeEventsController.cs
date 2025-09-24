using CavistaEventCelebration.Api.Dto.EmployeeEvent;
using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using CavistaEventCelebration.Api.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
            var userId = Guid.Parse(User.FindFirst("id").Value);
            var canApprove = User.IsInRole("SuperAdmin") || User.IsInRole("People");
            return Ok(await _eventService.AddEmployeeEvent(canApprove, userId, employeeEvent));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployeeEvent(UpdateEmployeeEventDto employeeEvent)
        {
            return Ok(await _eventService.UpdateEmployeeEvent(employeeEvent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeevent(Guid id)
        {
            return Ok(await _eventService.DeleteEmployeeEvent(id));
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<EmployeeEventDto>>> Get(int? index, int? pageSize, string? searchString)
        {
            Guid currentUserId = User.IsInRole("SuperAdmin") || User.IsInRole("People") ? Guid.Empty : Guid.Parse(User.FindFirst("id").Value);
            return Ok(await _eventService.EmployeeEvents(currentUserId, index, pageSize, searchString));
        }

        [Authorize(Roles = "SuperAdmin,People")]
        [HttpPut("approve-employee-event")]
        public async Task<IActionResult> ApproveEmployeeEvent([FromBody] List<Guid> employeeIds)
        {
            var userId = User?.FindFirstValue("id") != null && !string.IsNullOrEmpty(User?.FindFirstValue("id")) ? Guid.Parse(User?.FindFirstValue("id")) : Guid.Empty;
            return Ok(await _eventService.ApproveEmployeeEvent(userId, employeeIds));
        }
    }
}
