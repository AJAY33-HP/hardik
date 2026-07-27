I have a WIP (Work In Progress) Management System built with ASP.NET Core 8 Web API, Entity Framework Core (Code First), SQL Server, Repository Pattern, Service Layer, Swagger, and JWT Authentication.

Please update my backend according to the following requirements.

==========================
1. Checkout Approval Workflow
==========================

Current Workflow:

Employee
↓
Checkout Request
↓
Status = Pending
↓
Notification sent to Admin
↓
Admin Approves / Rejects
↓
If Approved:
    Inventory Quantity decreases
    Rack Occupied decreases
    Employee notified
If Rejected:
    Inventory unchanged
    Employee notified

Inventory must NOT decrease when employee submits checkout.

==========================
2. Checkout Endpoint
==========================

Create a CheckOutDto:

public class CheckOutDto
{
    public int WipInventoryId { get; set; }
    public int Quantity { get; set; }
    public int EmployeeId { get; set; }
}

Controller must use:

[HttpPost("checkout")]
public async Task<IActionResult> CheckOut([FromBody] CheckOutDto dto)

Frontend sends JSON.

==========================
3. Checkout Service
==========================

Validate:

• Quantity > 0
• Inventory exists
• Quantity <= Available Stock

Create Checkout record:

Status = Pending

Do NOT reduce inventory.

Create Audit History.

Save Checkout.

Create Notification.

Notification must contain:

Title

Message

EmployeeId

CheckOutId

RecipientRole = Admin

RecipientEmployeeId = null

Return success.

==========================
4. Notification
==========================

Notification table must store:

NotificationId

EmployeeId

RecipientRole

RecipientEmployeeId

CheckOutId

Title

Message

Timestamp

IsRead

Status

IsDeleted

NotificationCreateDto must support:

CheckOutId

Notification creation must include:

CheckOutId = checkout.CheckOutId

==========================
5. Admin Approval
==========================

Approve API:

POST

/api/Inventory/checkout/approve/{checkOutId}

Workflow:

Find Checkout

If Pending:

Reduce Inventory

Reduce Rack Occupied

Status = Approved

Audit

Create Employee Notification

Commit Transaction

Reject:

Status = Rejected

Inventory unchanged

Create Employee Notification

==========================
6. Inventory GetAll
==========================

Use:

.Include(Product)

.Include(Rack)

.ThenInclude(Warehouse)

Return DTO containing:

ProductName

ProductCode

RackCode

WarehouseName

Capacity

Occupied

Available

Status

Quantity

==========================
7. WipInventoryDto
==========================

Expand DTO:

ProductName

ProductCode

RackCode

WarehouseName

Capacity

Occupied

Available

Status

Quantity

==========================
8. Notification API
==========================

Admin should retrieve notifications using:

RecipientRole == "Admin"

Employee should retrieve notifications using:

RecipientEmployeeId

Notifications should include:

NotificationId

CheckOutId

Employee Name

Product Name

Quantity

Status

Date

==========================
9. Error Handling
==========================

No API should return HTTP 500 for notification failures.

Wrap notification creation in try/catch.

Log the error.

Return checkout success even if notification creation fails.

==========================
10. Deliverables
==========================

Update:

InventoryController

InventoryService

NotificationService

NotificationController

WipInventoryDto

CheckOutDto

Notification DTOs

All Entity Framework queries

Swagger endpoints

Ensure project builds successfully.
