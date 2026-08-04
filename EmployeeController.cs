using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wipmanagement.api.DTOs;
using wipmanagement.api.Interfaces.Services;

namespace wipmanagement.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("Registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] DTOs.EmployeeRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var res = await _employeeService.RegisterAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = res.EmployeeId }, res);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{employeeCode}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string employeeCode, [FromBody] DTOs.EmployeeUpdateDto dto)
        {
            if (string.IsNullOrEmpty(employeeCode) || employeeCode != dto.EmployeeCode)
                return BadRequest(new { message = "Employee code mismatch" });
            try
            {
                var res = await _employeeService.UpdateAsync(dto);
                return Ok(res);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{employeeCode}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string employeeCode)
        {
            var employee = await _employeeService.GetByCodeAsync(employeeCode);
            if (employee == null) return NotFound(new { message = "Employee not found" });

            var ok = await _employeeService.SoftDeleteAsync(employee.EmployeeId);
            if (!ok) return StatusCode(500, new { message = "Failed to delete employee" });
            return Ok(new { message = "Employee deleted successfully" });
        }

        [HttpPut("{employeeCode}/access")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccess(string employeeCode, [FromBody] EmployeeAccessDto accessDto)
        {
            if (string.IsNullOrEmpty(employeeCode))
                return BadRequest(new { message = "Employee code is required" });

            if (accessDto == null)
                return BadRequest(new { message = "Access data is required" });

            try
            {
                var result = await _employeeService.UpdateAccessAsync(employeeCode, accessDto);

                // If result is null, employee was not found
                if (result == null)
                    return NotFound(new { message = "Employee not found" });

                return Ok(new { message = "Access updated successfully", data = result });
            }
            catch (InvalidOperationException ex)
            {
                // If SaveChanges failed, return 500
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{employeeCode}/access")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAccess(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
                return BadRequest(new { message = "Employee code is required" });

            try
            {
                var result = await _employeeService.GetAccessAsync(employeeCode);
                if (result == null)
                    return NotFound(new { message = "Employee not found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto)
        {
            var created = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);
        }

        [HttpGet("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> GetById(int id)
        {
            var e = await _employeeService.GetByIdAsync(id);
            if (e == null) return NotFound();
            return Ok(e);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _employeeService.GetAllAsync();
            return Ok(list);
        }
    }
}

