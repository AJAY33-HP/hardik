I have an ASP.NET Core 8 Web API.

DO NOT modify any frontend code.

DO NOT create new APIs.

DO NOT redesign anything.

ONLY fix the Employee Access backend.

Current problems:

1. When the frontend opens "Edit Access", the backend must return the employee's current saved Access.
Currently the popup opens with all checkboxes empty.

2. When Admin saves Access, the database must update the Employee.Access column correctly.

3. After the employee logs in again, Login API must always return the latest Access from SQL Server.

4. JWT token must contain the latest Access claim.

5. LoginResponse must contain the latest Access.

6. Remove any cached or hardcoded Access values.

7. Do NOT change any existing API routes.

8. Do NOT change database schema.

9. Do NOT change authentication flow.

10. Only modify the files required for Employee Access.

After finishing provide:
- Modified files
- Why each file was modified
- How to test
I have a React frontend.

DO NOT modify backend code.

DO NOT redesign UI.

DO NOT modify Login page.

DO NOT modify Unauthorized page.

DO NOT modify routing.

DO NOT modify Sidebar behaviour.

ONLY fix Employee Edit Access.

Current problems:

1. When Admin clicks Edit Access,
the popup must automatically load the employee's existing permissions.

Currently all checkboxes are empty.

This is wrong.

2. If an employee already has Dashboard, Inventory and Reports permission,

those checkboxes must already be checked.

3. After Save Permissions succeeds,

do not change the current Admin sidebar.

do not refresh the Admin permissions.

Only update the selected employee.

4. Do not hardcode permissions.

Load permissions from backend response.

5. Save button behaviour should remain unchanged.

6. UI design should remain unchanged.

Only modify files related to Edit Access popup.

After finishing provide:
- Modified files
- Why each file was modified
- Testing steps.
