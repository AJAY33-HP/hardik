using System.Security.Cryptography;
using System.Text;
using wipmanagement.api.DTOs;
using wipmanagement.api.Interfaces.Repositories;
using wipmanagement.api.Interfaces.Services;
using wipmanagement.api.Models;

namespace wipmanagement.api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Updates an existing employee from EmployeeUpdateDto
        // Validates that employeeCode and email are still unique (excluding current employee)
        // Only updates password if provided, preserves existing Access unless explicitly updated
        // Returns the updated employee as EmployeeResponse with Access included
        public async Task<EmployeeResponse> UpdateAsync(DTOs.EmployeeUpdateDto dto)
        {
            // Retrieve existing employee by ID
            var existing = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (existing == null) throw new InvalidOperationException("Employee not found");

            // Verify EmployeeCode is unique (if changing the code)
            if (existing.EmployeeCode != dto.EmployeeCode && await _employeeRepository.ExistsByCodeAsync(dto.EmployeeCode))
                throw new InvalidOperationException("EmployeeCode already exists");

            // Verify Email is unique (if changing the email)
            if (existing.Email != dto.Email && await _employeeRepository.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException("Email already exists");

            // Update employee properties
            existing.EmployeeCode = dto.EmployeeCode;
            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.Department = dto.Department;
            existing.Role = dto.Role;
            existing.Shift = dto.Shift;

            // Only update password if provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                existing.PasswordHash = HashPassword(dto.Password!);
            }

            // Update Access only if provided, otherwise preserve existing Access
            if (!string.IsNullOrWhiteSpace(dto.Access))
            {
                existing.Access = dto.Access;
            }

            _employeeRepository.Update(existing);
            await _employeeRepository.SaveChangesAsync();

            return new EmployeeResponse
            {
                EmployeeId = existing.EmployeeId,
                EmployeeCode = existing.EmployeeCode,
                Name = existing.Name,
                Email = existing.Email,
                Department = existing.Department,
                Role = existing.Role,
                Shift = existing.Shift,
                IsActive = existing.IsActive,
                CreatedAt = existing.CreatedAt,
                // Return updated Access permissions
                Access = existing.Access ?? string.Empty
            };
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var existing = await _employeeRepository.GetByIdAsync(id);
            if (existing == null) return false;
            existing.IsActive = false; // mark inactive
            _employeeRepository.Update(existing);
            await _employeeRepository.SaveChangesAsync();
            return true;
        }

        // Creates a new employee from EmployeeCreateDto
        // Validates that employeeCode and email don't already exist
        // Returns the created employee as EmployeeDto with default empty Access
        public async Task<EmployeeDto> CreateAsync(EmployeeCreateDto dto)
        {
            // Verify EmployeeCode is unique
            if (await _employeeRepository.ExistsByCodeAsync(dto.EmployeeCode))
                throw new InvalidOperationException("Employee code already exists");

            var employee = new Employee
            {
                EmployeeCode = dto.EmployeeCode,
                Name = dto.Name,
                Email = dto.Email,
                Department = dto.Department,
                Role = dto.Role,
                Shift = dto.Shift,
                IsActive = true,
                PasswordHash = HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                // New employees start with empty Access permissions
                Access = string.Empty
            };

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                Role = employee.Role,
                Shift = employee.Shift,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                // Return default empty Access for new employee
                Access = employee.Access ?? string.Empty
            };
        }

        // Retrieves all employees from the database and maps them to EmployeeDto
        // with Access permissions included
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var all = await _employeeRepository.GetAllAsync();
            return all.Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeCode = e.EmployeeCode,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department,
                Role = e.Role,
                Shift = e.Shift,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt,
                // Map comma-separated Access permissions string
                Access = e.Access ?? string.Empty
            });
        }

        // Retrieves a single employee by their unique ID and maps to EmployeeDto
        // Returns null if employee not found
        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var e = await _employeeRepository.GetByIdAsync(id);
            if (e == null) return null;

            return new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeCode = e.EmployeeCode,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department,
                Role = e.Role,
                Shift = e.Shift,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt,
                // Map comma-separated Access permissions string
                Access = e.Access ?? string.Empty
            };
        }

        // Retrieves a single employee by their employee code and maps to EmployeeDto
        // Returns null if employee not found
        public async Task<EmployeeDto?> GetByCodeAsync(string code)
        {
            var e = await _employeeRepository.GetByCodeAsync(code);
            if (e == null) return null;

            return new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeCode = e.EmployeeCode,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department,
                Role = e.Role,
                Shift = e.Shift,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt,
                // Map comma-separated Access permissions string
                Access = e.Access ?? string.Empty
            };
        }

        // Registers a new employee from EmployeeRegisterRequest
        // Validates that employeeCode and email are unique
        // Returns the registered employee as EmployeeResponse with default empty Access
        public async Task<EmployeeResponse> RegisterAsync(DTOs.EmployeeRegisterRequest request)
        {
            // Verify EmployeeCode is unique
            if (await _employeeRepository.ExistsByCodeAsync(request.EmployeeCode))
                throw new InvalidOperationException("EmployeeCode already exists");

            // Verify Email is unique
            if (await _employeeRepository.ExistsByEmailAsync(request.Email))
                throw new InvalidOperationException("Email already exists");

            var employee = new Employee
            {
                EmployeeCode = request.EmployeeCode,
                Name = request.Name,
                Email = request.Email,
                Department = request.Department,
                Role = request.Role,
                Shift = request.Shift,
                IsActive = true,
                PasswordHash = HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                // New employees start with empty Access permissions
                Access = string.Empty
            };

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return new EmployeeResponse
            {
                EmployeeId = employee.EmployeeId,
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                Role = employee.Role,
                Shift = employee.Shift,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                // Return default empty Access for new employee
                Access = employee.Access ?? string.Empty
            };
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // Retrieves an employee's current access permissions and converts to EmployeeAccessDto
        // Parses comma-separated Access string from database to boolean flags
        // Example: "Dashboard,Inventory,Reports" → Dashboard=true, Inventory=true, Reports=true, others=false
        // Returns EmployeeAccessDto with boolean flags for each module
        public async Task<EmployeeAccessDto?> GetAccessAsync(string employeeCode)
        {
            var employee = await _employeeRepository.GetByCodeAsync(employeeCode);
            if (employee == null) return null;

            // Parse the comma-separated access string into a set for quick lookup
            var accessModules = string.IsNullOrWhiteSpace(employee.Access)
                ? new HashSet<string>()
                : new HashSet<string>(employee.Access.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)));

            // Convert to EmployeeAccessDto with boolean flags
            return new EmployeeAccessDto
            {
                Dashboard = accessModules.Contains("Dashboard"),
                Products = accessModules.Contains("Products"),
                Employees = accessModules.Contains("Employees"),
                Wip = accessModules.Contains("Wip"),
                Inventory = accessModules.Contains("Inventory"),
                CheckIn = accessModules.Contains("CheckIn"),
                CheckOut = accessModules.Contains("CheckOut"),
                Reports = accessModules.Contains("Reports"),
                Racks = accessModules.Contains("Racks"),
                Notifications = accessModules.Contains("Notifications"),
                Prediction = accessModules.Contains("Prediction")
            };
        }

        // Updates employee access permissions from EmployeeAccessDto
        // Converts boolean access flags to comma-separated string format
        // Example: true Dashboard + true Inventory = "Dashboard,Inventory"
        // Returns the updated employee as EmployeeResponse with Access included
        // Returns null if employee not found
        public async Task<EmployeeResponse?> UpdateAccessAsync(string employeeCode, EmployeeAccessDto accessDto)
        {
            // Verify employee exists and fetch current record
            var employee = await _employeeRepository.GetByCodeAsync(employeeCode);
            if (employee == null) return null;

            // Convert access DTO boolean flags to comma-separated string
            // Only includes modules where access is true
            var accessList = new List<string>();

            if (accessDto.Dashboard) accessList.Add("Dashboard");
            if (accessDto.Products) accessList.Add("Products");
            if (accessDto.Employees) accessList.Add("Employees");
            if (accessDto.Wip) accessList.Add("Wip");
            if (accessDto.Inventory) accessList.Add("Inventory");
            if (accessDto.CheckIn) accessList.Add("CheckIn");
            if (accessDto.CheckOut) accessList.Add("CheckOut");
            if (accessDto.Reports) accessList.Add("Reports");
            if (accessDto.Racks) accessList.Add("Racks");
            if (accessDto.Notifications) accessList.Add("Notifications");
            if (accessDto.Prediction) accessList.Add("Prediction");

            // Format: "Dashboard,Inventory,Reports" or empty string if no access granted
            var newAccessValue = string.Join(",", accessList);

            // Update the employee's access
            employee.Access = newAccessValue;
            _employeeRepository.Update(employee);

            // Persist to database and verify the update
            int rowsAffected = await _employeeRepository.SaveChangesWithResultAsync();

            // Verify the database was actually updated
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Failed to update employee access in database");
            }

            return new EmployeeResponse
            {
                EmployeeId = employee.EmployeeId,
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                Role = employee.Role,
                Shift = employee.Shift,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                // Return updated comma-separated Access permissions
                Access = employee.Access ?? string.Empty
            };
        }

    }
}

