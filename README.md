Fix only the frontend permission system.

Current issue:

1. Backend login API is already working correctly.
2. Login API returns:
   token,
   role,
   employeeId,
   employeeCode,
   name,
   access

3. The access field is stored in the Employees.Access column as a JSON string like:

{
  "dashboard": true,
  "inventory": false,
  "products": true,
  "employees": false,
  "wip": true,
  "checkIn": true,
  "checkOut": false,
  "reports": false,
  "notifications": true,
  "prediction": false,
  "racks": false
}

Backend must NOT be modified.

Only fix the React frontend.

Tasks:

1. auth.js
- Detect whether access is a JSON string.
- Parse it using JSON.parse().
- Keep backward compatibility if comma-separated access exists.
- hasPermission() must always use the parsed JSON object.

2. Login.jsx
- Store response.data.access exactly as received.
- Do not convert or modify it.
- Clear old localStorage access before storing new access.

3. Sidebar.jsx
- Build sidebar ONLY from parsed access object.
- Remove role-based menu filtering.
- Show menu only if access value is true.
- Do not change sidebar UI or styling.

4. App.jsx
- Replace RequireRole usage with ProtectedRoute.
- Protect routes using permissions:
Dashboard -> dashboard
Inventory -> inventory
Products -> products
Employees -> employees
WIP -> wip
CheckIn -> checkIn
CheckOut -> checkOut
Reports -> reports
Notifications -> notifications
Prediction -> prediction
Racks -> racks

5. ProtectedRoute
- Read permissions from auth.js.
- Redirect to /unauthorized only if permission is false.
- Do not use role-based authorization.

Important:
- Do NOT modify backend.
- Do NOT modify API.
- Do NOT modify database.
- Do NOT modify UI design.
- Do NOT create any .md, README, notes, documentation, or extra files.
- Modify only existing React files.
- Keep all existing functionality unchanged except fixing permission handling.
