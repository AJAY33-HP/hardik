using Microsoft.EntityFrameworkCore;
using wipmanagement.api.Data;
using wipmanagement.api.Interfaces.Repositories;
using wipmanagement.api.Models;

namespace wipmanagement.api.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Employee?> GetByCodeAsync(string employeeCode)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
        }

        public async Task<bool> ExistsByCodeAsync(string employeeCode)
        {
            return await _dbSet.AnyAsync(e => e.EmployeeCode == employeeCode);
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(e => e.Email == email);
        }
    }
}
