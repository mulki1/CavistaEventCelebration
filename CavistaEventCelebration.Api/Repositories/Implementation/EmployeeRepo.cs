using CavistaEventCelebration.Api.Data;
using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CavistaEventCelebration.Api.Repositories.Implementation
{

    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly AppDbContext _db;
        public EmployeeRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Add(Employee employee)
        {
            try
            {
                await _db.Employees.AddAsync(employee);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }

        }

        public async Task<bool> UploadEmployee(List<Employee> employees)
        {
            try
            {
                await _db.Employees.AddRangeAsync(employees);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }

        }

        public async Task<List<Employee>> Get()
        {
            return await _db.Employees.Where(e => !e.IsDeprecated).AsNoTracking().ToListAsync();
        }

        public async Task<bool> Remove(Employee employee)
        {
            try
            {
                employee.IsDeprecated = true;
                var result = _db.Employees.Update(employee);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public async Task<Employee> GetById(Guid id)
        {
            try
            {
                return await _db.Employees.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeprecated);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            try
            {
                _db.Employees.Update(employee);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }
    }
}
