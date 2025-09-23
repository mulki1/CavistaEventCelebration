using CavistaEventCelebration.Api.Dto;
using CavistaEventCelebration.Api.Dto.Employee;
using CavistaEventCelebration.Api.Dto.EmployeeEvent;
using CavistaEventCelebration.Api.Models;

namespace CavistaEventCelebration.Api.Services.Interface
{
    public interface IEmployeeService
    {
        Task<Response<bool>> AddEmployee(AddEmployeeDto employee);
        Task UploadEmployee(string filePath);
        Task<Response<List<Employee>>> Get();
        Task<Response<bool>> DeleteEmployee(Guid id);
        Task<Response<bool>> UpdateEmployee(UpdateEmployeeDto employee);
    }
}
