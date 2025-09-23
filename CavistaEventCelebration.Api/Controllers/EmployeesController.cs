using CavistaEventCelebration.Api.Dto.Employee;
using CavistaEventCelebration.Api.Dto.EmployeeEvent;
using CavistaEventCelebration.Api.Services.Interface;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CavistaEventCelebration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)

        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employeeService.Get());
        }

        [HttpPost]
        public async Task<ActionResult<AddEmployeeDto>> Create([FromBody] AddEmployeeDto employee)
        {
            return Ok( await _employeeService.AddEmployee(employee));
        }

        [HttpPut]
        public async Task<ActionResult<AddEmployeeDto>> Update([FromBody] UpdateEmployeeDto employee)
        {

            return Ok(await _employeeService.UpdateEmployee(employee));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AddEmployeeDto>> Delete(Guid id)
        {

            return Ok(await _employeeService.DeleteEmployee(id));
        }

        [HttpPost("upload-excel")]
        public IActionResult ImportEmployees(IFormFile file, [FromServices] IBackgroundJobClient backgroundJobs)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            var filePath = Path.Combine(Path.GetTempPath(), file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            backgroundJobs.Enqueue<IEmployeeService>(service => service.UploadEmployee(filePath));

            return Ok(new { Message = "Employee import job has been queued." });
        }
    }
}
