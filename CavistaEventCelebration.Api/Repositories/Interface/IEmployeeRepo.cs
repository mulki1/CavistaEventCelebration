using CavistaEventCelebration.Api.Models;

namespace CavistaEventCelebration.Api.Repositories.Interface
{
    public interface IEmployeeRepo
    {
        Task<bool> Add(Employee employee);
        Task<bool> UploadEmployee(List<Employee> employees);
        Task<List<Employee>> Get();
        Task<bool> Remove(Employee employee);
        Task<Employee> GetById(Guid id);
        Task<bool> UpdateEmployee(Employee employee);
    }
}
