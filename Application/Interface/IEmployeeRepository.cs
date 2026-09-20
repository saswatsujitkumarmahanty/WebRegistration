using Application.Dto;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(Guid id);
        Task<Employee> AddEmployeeAsync(AddEmployeeDto addEmployeeDto);
        Task<UpdateEmployeeDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto updateEmployeeDto);
        Task<bool> DeleteEmployeeAsync(Guid id);
    }
}
