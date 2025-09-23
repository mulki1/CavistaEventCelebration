﻿using CavistaEventCelebration.Api.Dto.EmployeeEvent;
using CavistaEventCelebration.Api.Models;

namespace CavistaEventCelebration.Api.Services.Interface
{
    public interface IEmployeeService
    {
        bool AddEmployee(EmployeeDto employee);
        Task UploadEmployee(string filePath);
        Task<List<Employee>> Get();
    }
}
