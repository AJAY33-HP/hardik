Redesign the Notifications page only. Do not modify any backend API, controllers, services, database, or business logic.

Requirements:

1. Replace the current notification cards with a professional table layout.

Columns:
- Notification Type
- Employee
- Product
- Quantity
- Status
- Date & Time
- Action

2. Only checkout requests with Status = Pending should show a "View" button.

3. Clicking the "View" button should open a right-side drawer (or modal) displaying:

- Employee Name
- Employee ID
- Product Name
- Product Code
- Warehouse
- Rack
- Requested Quantity
- Available Quantity
- Request Time
- Current Status

At the bottom show:
- Approve button (green)
- Reject button (red)

4. After Approve or Reject:
- Show a success toast.
- Close the drawer.
- Remove the approved/rejected request from the Pending list without refreshing the page.
- Reload the notifications automatically.

5. Fix all empty values.
Do not display blank Employee, Product, Quantity, Warehouse, Rack or Status.
Map the API response correctly.
If any value is missing, display "N/A" instead of leaving it blank.

6. Improve the UI:
- Modern enterprise design.
- Sticky table header.
- Hover effect on rows.
- Status badges (Pending, Approved, Rejected, CheckIn).
- Bell icon for notification type.
- Responsive layout.
- Pagination if notifications exceed 10.
- Search should filter the table.

7. Do not show Approve/Reject buttons directly in the table.
They should appear only inside the drawer after clicking View.

8. Keep all existing API endpoints exactly the same.
Only change the React frontend.

9. Ensure the frontend correctly maps the backend response fields so Employee, Product, Quantity, Warehouse, Rack, Status and Time are displayed instead of empty values.

10. Do not change any backend code or API contract.
