Analyze the existing ASP.NET Core Web API project and fix ONLY the Employee Access Permission functionality.

IMPORTANT RULES:
- DO NOT create any new .md files.
- DO NOT create documentation files.
- DO NOT create README files.
- DO NOT change unrelated modules.
- DO NOT modify frontend code.
- Fix only the backend.

Current Issue:

1. Employee Edit is working.

2. Edit Access popup opens correctly.

3. Saving access returns:
"Access updated successfully"

4. But the Access column is NOT updated for some employees (example: EmployeeCode = E1).

5. SQL shows:
EmployeeCode = E1
Access = NULL / Empty

while other employees (admin, EMP001, EMP002) have JSON stored in Access column.

6. Login API returns:

{
  "role":"Admin",
  "access":""
}

because Access is empty.

Due to this the frontend sidebar becomes empty after login.

Tasks:

1. Find the PUT API responsible for

PUT /api/Employee/{employeeCode}/access

2. Trace the complete flow:

Controller
→ Service
→ Repository (if exists)
→ Entity Framework
→ Database Save

3. Find why SaveChangesAsync() is not updating Access for some employees.

4. Verify EmployeeCode lookup.

5. Ensure employee is fetched correctly.

6. Ensure employee.Access is assigned before SaveChangesAsync().

7. Ensure SaveChangesAsync() actually affects one row.

8. If employee not found, return proper 404 instead of success.

9. If SaveChangesAsync returns 0 rows, return failure.

10. Return success only after database update succeeds.

11. Preserve existing JSON format already used by admin/EMP001.

12. Do NOT change JWT authentication.

13. Do NOT change login API except making sure it returns the updated Access value from database.

14. Do NOT modify any other APIs.

15. Keep all existing routes unchanged.

16. After fixing, verify this scenario:

Admin Login
↓
Edit Access for employee E1
↓
Save
↓
SQL Access column contains updated JSON
↓
Employee Login
↓
Login response contains updated Access JSON
↓
Frontend automatically shows permitted sidebar menus.

Output:
Only modify the required backend files.
Do not generate markdown files.
Do not create documentation.
Do not change unrelated code.
