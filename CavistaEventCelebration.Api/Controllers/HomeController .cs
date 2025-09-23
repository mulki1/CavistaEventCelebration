using System.Security.Claims;
using CavistaEventCelebration.Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CavistaEventCelebration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IDashBoardService _service;
        public HomeController(IDashBoardService service)

        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _service.Get();
            result.UserId = User?.FindFirstValue("id") != null && !string.IsNullOrEmpty(User?.FindFirstValue("id")) ? Guid.Parse(User?.FindFirstValue("id")) : Guid.Empty;
            result.UserEmail = User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            result.UserName = User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            return Ok(await _service.Get());
        }
    }
}
