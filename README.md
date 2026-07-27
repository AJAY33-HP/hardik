Do not use a separate Checkout Requests page.

Keep the existing Notification page as the approval page.

For every Checkout Request notification:
- Save CheckOutId in the Notification table.
- Return CheckOutId in the Notification API.
- Approve button should call:
  POST /api/Inventory/checkout/approve/{checkOutId}
- Reject button should call:
  POST /api/Inventory/checkout/reject/{checkOutId}

On Approve:
- Update Checkout Status to Approved.
- Reduce Inventory Quantity.
- Reduce Rack Occupied.
- Create notification for the employee.

On Reject:
- Update Checkout Status to Rejected.
- Inventory should not change.
- Create notification for the employee.

Do not change existing APIs unnecessarily.
Keep backward compatibility with the current frontend.


Remove the Checkout Requests page.

Use the existing Notifications page for Admin approvals.

For every Checkout Request notification:
- Show Employee Name.
- Show Product.
- Show Quantity.
- Show Status.
- Display Approve and Reject buttons.

Approve button:
POST /api/Inventory/checkout/approve/{checkOutId}

Reject button:
POST /api/Inventory/checkout/reject/{checkOutId}

After approval or rejection:
- Refresh Notifications.
- Refresh Inventory.
- Refresh Dashboard.
- Show success message.

Do not change the existing notification UI except adding Approve and Reject buttons.
