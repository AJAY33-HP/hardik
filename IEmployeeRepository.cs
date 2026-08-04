using wipmanagement.api.Models;

namespace wipmanagement.api.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetByCodeAsync(string employeeCode);
        Task<bool> ExistsByCodeAsync(string employeeCode);
        Task<Employee?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
    }
}