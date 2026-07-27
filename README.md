Update my ASP.NET Core 8 Web API for the WIP Management System.

Requirements:

1. Employee checkout should create a Checkout Request with Status = Pending.
2. Do NOT reduce inventory when the request is submitted.
3. Create an Admin notification for every checkout request.
4. Save CheckOutId in the Notification table.
5. Admin should approve or reject checkout requests.
6. On Approve:
   - Reduce inventory quantity.
   - Reduce rack occupied quantity.
   - Change checkout status to Approved.
   - Send notification to the employee.
7. On Reject:
   - Change checkout status to Rejected.
   - Inventory should remain unchanged.
   - Send notification to the employee.
8. Update Inventory GetAll API to include Product, Rack, and Warehouse details using Include() and ThenInclude().
9. Return ProductName, ProductCode, RackCode, WarehouseName, Capacity, Occupied, Available, and Status in WipInventoryDto.
10. Handle exceptions properly and never return HTTP 500 for notification failures.
11. Remove duplicate Checkout Request APIs/controllers if present.
12. Ensure all APIs work correctly in Swagger without errors.

Update my React + Bootstrap frontend for the WIP Management System.

Requirements:

1. Checkout page should submit a Pending Checkout Request.
2. Employee ID should be read from localStorage.
3. Display Product, Rack, Warehouse, Capacity, Occupied, Available, and Status.
4. Validate quantity before submitting.
5. Show success message: "Checkout Request Submitted. Waiting for Admin Approval."
6. Create a separate "Checkout Requests" page for Admin.
7. Admin should Approve or Reject requests from that page.
8. Notification page should only display notifications and should not contain Approve/Reject buttons.
9. Refresh Dashboard, Inventory, Notifications, and Checkout Requests after every approval or rejection.
10. Use a clean, responsive Bootstrap UI with proper loading indicators and toast messages.
