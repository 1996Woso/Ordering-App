using Ordering_App.Models;
using Ordering_App.Models.Domain;
using Ordering_App.Models.DTOs;

namespace Ordering_App.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> CreateAsync(AddEmployeeDTO employeeDTO);
        Task<Employee> UpdateAsync(UpdateEmployeeDTO updateEmployeeDTO);
        Task<Employee> DeleteAsync(Employee employee);
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee?> GetByEmpoyeeNoAsync(Guid employeeNo);
        Task<PagedList<Employee>> GetAllAsync(UserParams userParams);
        Task<bool> SaveAllAsync();
        Task<bool> UserExistsAsync(string name);
        Task<string> ConvertMonthAsync(string inpu);
    }
}
