Update the Notification backend.

Requirements:

1. The Notification API should return complete notification details from the SQL Server Notifications table.

2. Join related tables (CheckOuts, Employees, WipInventories, Products) so every notification contains:
   - NotificationId
   - CheckOutId
   - EmployeeId
   - EmployeeName
   - ProductId
   - ProductName
   - Quantity
   - Status
   - Title
   - Message
   - Timestamp
   - IsRead

3. When Admin approves or rejects a checkout request:
   - Update Checkout.Status.
   - Update Notification.Status.
   - Mark the notification as processed (or IsRead = true).
   - Save all changes.

4. Return updated notification data after every operation.

5. Do not change existing API routes.


Update the Notifications page.

Requirements:

1. Load notifications only from the Notification API.

2. Display:
   - Employee Name
   - Product Name
   - Quantity
   - Status
   - Date & Time

3. Show Approve and Reject buttons only when Status = Pending.

4. If Status = Approved or Rejected:
   - Hide the buttons.
   - Show a green Approved badge or a red Rejected badge.

5. After Approve or Reject:
   - Call the API.
   - Refresh the notification list.
   - Refresh Inventory.
   - Refresh Dashboard.

6. Do not use local state as the source of truth. Always reload from the backend after every action.
