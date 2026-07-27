Update the Notifications page.

The Approve and Reject buttons should NOT call:

POST /api/Inventory/checkout/approve/{checkOutId}
POST /api/Inventory/checkout/reject/{checkOutId}

Instead, call:

POST /api/CheckOutRequests/{id}/approve?adminEmployeeId={adminEmployeeId}

POST /api/CheckOutRequests/{id}/reject?adminEmployeeId={adminEmployeeId}

Use the Notification.CheckOutId as the {id} value.

If the API returns HTTP 200 or HTTP 204, treat it as success.

After success:
- Show a success toast.
- Refresh notifications.
- Refresh inventory.
- Refresh dashboard.

Do not call the Inventory approve/reject endpoints.
