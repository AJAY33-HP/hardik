Fix the Notification GET API only.

Do not change the frontend.

Do not change the Notification table schema.

The Notification table should remain lightweight and continue storing only notification-related fields such as NotificationId, Title, Message, CheckOutId, RecipientRole, Status and Timestamp.

When GET /api/Notification is called:

1. Load notifications.

2. If CheckOutId is not null:
   - Join with CheckOuts.
   - Join CheckOuts with Employees.
   - Join CheckOuts with WipInventory.
   - Join WipInventory with Products.
   - Join Rack and Warehouse.

3. Return a NotificationResponseDto containing:

- NotificationId
- NotificationType
- Title
- Message
- CheckOutId
- EmployeeId
- EmployeeName
- ProductId
- ProductCode
- ProductName
- Quantity
- WarehouseName
- RackCode
- Status
- RecipientRole
- Timestamp
- IsRead

4. If a notification is not related to a checkout (CheckOutId is null), return the available notification fields and use null only for the checkout-related properties.

5. Update AutoMapper or manual mapping so these fields are populated correctly.

6. Ensure GET /api/Notification never returns empty EmployeeName, ProductName or Quantity for checkout notifications when the related data exists in the database.

7. Do not modify Approve/Reject logic, Checkout logic or database schema. Only improve the GET Notification API response by loading related data through Entity Framework Include/ThenInclude or LINQ joins.

Update only the React frontend Notification page.

Do not modify any backend code or API endpoints.

The backend now returns the following fields:

- notificationId
- notificationType
- title
- message
- checkOutId
- employeeId
- employeeName
- productId
- productCode
- productName
- quantity
- warehouseName
- rackCode
- status
- recipientRole
- timestamp
- isRead

Requirements:

1. Display the notification list in a modern responsive table.

Columns:
- Type
- Employee
- Product
- Quantity
- Warehouse
- Rack
- Status
- Date & Time
- Action

2. Map the backend response correctly:
- Employee → employeeName
- Product → productName
- Quantity → quantity
- Warehouse → warehouseName
- Rack → rackCode
- Status → status
- Date → timestamp

3. Do not show "N/A" if the backend sends a valid value.

4. Only show a "View" button when:
- notificationType is "Checkout Request"
- status is "Pending"

5. Clicking "View" opens a right-side drawer (or modal) showing:

- Employee Name
- Employee ID
- Product Name
- Product Code
- Warehouse
- Rack
- Requested Quantity
- Status
- Date & Time

6. Inside the drawer show:
- Green Approve button
- Red Reject button

7. After Approve or Reject:
- Show success toast.
- Close the drawer.
- Remove the processed request from the Pending list.
- Refresh the notification list automatically.

8. Show colored badges:
- Pending → Orange
- Approved → Green
- Rejected → Red
- CheckIn → Blue
- Unread → Gray

9. Add:
- Search
- Pagination
- Loading spinner
- "No notifications found" message when empty.

10. Keep the existing API URLs unchanged.
Only update the UI and data binding.
