using CavistaEventCelebration.Api.Dto;
using CavistaEventCelebration.Api.Dto.Employee;
using CavistaEventCelebration.Api.Dto.EmployeeEvent;
using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Repositories.Interface;
using CavistaEventCelebration.Api.Services.Interface;
using ClosedXML.Excel;

namespace CavistaEventCelebration.Api.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _repo;
        public EmployeeService(IEmployeeRepo repo)
        {
            _repo = repo;
        }

        public  async Task<Response<bool>> AddEmployee(AddEmployeeDto employee)
        {
            if (employee == null)
            {
                return Response<bool>.Failure("Employee can not be null");
            }
            var _employee = new Employee()
            {
                Id = new Guid(),
                EmailAddress = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName
            };
            var resonse =  await _repo.Add(_employee);
            if (resonse)
            {
                return Response<bool>.Success(resonse, "Employee created");
            }

            return Response<bool>.Failure("Could not create employee");
        }

        public async Task UploadEmployee(string filePath)
        {
            try
            {
                using var workbook = new XLWorkbook(filePath);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed();
                var employees = new List<Employee>();
                foreach (var row in rows.Skip(1)) 
                {
                    var firstName = row.Cell(1).GetString();
                    var lastName = row.Cell(2).GetString();
                    var email = row.Cell(3).GetString();
                    if (string.IsNullOrWhiteSpace(email)) continue;
                    employees.Add(new Employee
                    {
                        Id = Guid.NewGuid(),
                        FirstName = firstName,
                        LastName = lastName,
                        EmailAddress = email,
                        IsDeprecated = false
                    });
                }

                await _repo.UploadEmployee(employees);
                Console.WriteLine($" Imported {employees.Count} employees successfully from {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Employee import failed: {ex.Message}");
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        public async Task<PaginatedList<Employee>> Get(int? index, int? pageSize, string? searchString)
        {
            var empResp = new List<Employee>();
            var result = new PaginatedList<Employee>(empResp, 0, 1, 10);
            var employees =  _repo.Get();
            if(employees != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    employees = employees.Where(e => e.LastName.ToLower().Contains(searchString.ToLower())
                    || e.FirstName.ToLower().Contains(searchString.ToLower())
                    || e.EmailAddress.ToLower().Contains(searchString.ToLower())
                    );
                }

                result = await PaginatedList<Employee>.CreateAsync(employees, index ?? 1, pageSize ?? 10);
                return result;

            }

            return result;
        }

        public async Task<Response<bool>> DeleteEmployee(Guid id)
        {
            var employee = await _repo.GetById(id);
            if (employee == null)
            {
                return Response<bool>.Failure("Employee does not exist");
            }
            var result = await _repo.Remove(employee);

            if (result)
            {
                return Response<bool>.Success(result, "Employee Deleted");
            }
            return Response<bool>.Failure("Could not delete employee, try again later");
        }

        public async Task<Response<bool>> UpdateEmployee(UpdateEmployeeDto employee)
        {
            if (employee == null)
            {
                return Response<bool>.Failure("Employee can not be null");
            }
            var existingEmployee = await _repo.GetById(employee.Id);
            if (existingEmployee == null)
            {
                return Response<bool>.Failure("Employee does not exist");
            }
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            var result = await _repo.UpdateEmployee(existingEmployee);
            if (result)
            {
                return Response<bool>.Success(true, "Employee updated");
            }
            return Response<bool>.Failure("Employee could not be update, please try again");
        }

    }
}
