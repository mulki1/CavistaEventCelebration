using CavistaEventCelebration.Api.Dto.Event;
using CavistaEventCelebration.Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CavistaEventCelebration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _eventService.Events());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EventDto newEvent)
        {
            return Ok(await _eventService.AddEvent(newEvent));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EventDto updateEvent)
        {
            return Ok(await _eventService.UpdateEvent(id, updateEvent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _eventService.DeleteEvent(id));
        }
    }
}
