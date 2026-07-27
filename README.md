I have a React + Bootstrap frontend connected to an ASP.NET Core Web API.

Please update the frontend according to the following requirements.

==========================
1. Checkout Page
==========================

Design a professional manufacturing checkout page.

Flow:

Inventory Dropdown

↓

Inventory Details Card

↓

Employee ID (Readonly)

↓

Checkout Quantity

↓

Destination

↓

Remaining Stock

↓

Submit

Inventory Card must display:

Product

Product Code

Rack

Warehouse

Capacity

Occupied

Available

Status

Progress Bar

Employee ID should come from:

localStorage.getItem("employeeId")

Never ask user to type Employee ID.

==========================
2. Validation
==========================

Quantity > 0

Quantity <= Available Stock

Disable button if invalid.

Show Remaining Stock live.

==========================
3. Checkout API
==========================

Send JSON:

{
  "wipInventoryId":1,
  "quantity":10,
  "employeeId":3
}

Content-Type:

application/json

Success Message:

Checkout Request Submitted Successfully.

Waiting for Admin Approval.

==========================
4. Inventory API
==========================

Read:

ProductName

ProductCode

RackCode

WarehouseName

Capacity

Occupied

Available

Status

Quantity

Bind correctly.

No blank fields.

==========================
5. Admin
==========================

Do NOT approve from Notification page.

Create a new page:

Checkout Requests

Menu:

Dashboard

Inventory

CheckIn

CheckOut

Checkout Requests

Notifications

Reports

Prediction

==========================
6. Checkout Requests Page
==========================

Table:

Request ID

Employee

Product

Rack

Warehouse

Quantity

Status

Date

Actions

Approve

Reject

Approve calls:

POST

/api/Inventory/checkout/approve/{checkOutId}

Reject calls:

POST

/api/Inventory/checkout/reject/{checkOutId}

After approval:

Refresh:

Inventory

Dashboard

Checkout Requests

Notifications

==========================
7. Notifications
==========================

Notification page only displays notifications.

No Approve button.

Cards should show:

Title

Employee

Product

Quantity

Status

Date

Unread Badge

==========================
8. Dashboard
==========================

Refresh automatically after:

CheckIn

Checkout Approval

Checkout Rejection

Inventory changes

==========================
9. UI
==========================

Use Bootstrap 5.

Responsive.

Professional warehouse dashboard.

Modern cards.

Icons.

Loading spinner.

Toast notifications.

Confirmation dialogs.

==========================
10. Deliverables
==========================

Update:

Checkout.jsx

Inventory.jsx

Dashboard.jsx

Notifications.jsx

Create:

CheckoutRequests.jsx

Update React Router

Update Sidebar

Update API services

Ensure all pages work with the ASP.NET Core backend.
