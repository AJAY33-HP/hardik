import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

   import Login from "./pages/Login";
import Register from "./pages/Register";
import Dashboard from "./pages/Dashboard";
import Inventory from "./pages/Inventory";
import Products from "./pages/Products";
     import WIP from "./pages/WIP";
import Employees from "./pages/Employees";
import CheckIn from "./pages/CheckIn";
import CheckOut from "./pages/CheckOut";
import Notifications from "./pages/Notifications";
import Racks from "./pages/Racks";
import Reports from "./pages/Reports";
import ProductMovementReport from "./pages/ProductMovementReport";
import EmployeeActivityReport from "./pages/EmployeeActivityReport";
import WarehouseReport from "./pages/WarehouseReport";
import RackUtilizationReport from "./pages/RackUtilizationReport";
import Predictions from "./pages/Prediction";
import Profile from "./pages/Profile";
import RackProducts from "./pages/RackProducts";

import Navbar from "./components/Navbar";
import Sidebar from "./components/Sidebar";
import { getCurrentUser } from "./utils/auth";


function RequireRole({ allowed, children }) {
    const user = getCurrentUser();
    const role = user?.role;
    if (!allowed || allowed.length === 0) return children; // no restriction
    if (allowed.includes(role)) return children;
    // redirect to unauthorized page
    return <Navigate to="/unauthorized" replace />;
}

function Unauthorized() {
    return (
        <div className="container-fluid p-4">
            <div className="alert alert-danger">You are not authorized to view this page.</div>
        </div>
    );
}

function Layout({ children }) {
    return (
        <div className="d-flex">
            <Sidebar />

            <div className="flex-grow-1">
                <Navbar />

                {children}
            </div>
        </div>
    );
}
function App() {
    return (
        <BrowserRouter>

            <Routes>

                {/* Login */}
           
                <Route path="/" element={<Login />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />

                {/* Main Pages */}

                <Route path="/dashboard" element={<Layout> <Dashboard /> </Layout>} />
                <Route path="/products" element={<Layout> <Products/> </Layout>} />
                <Route path="/employees" element={<Layout> <RequireRole allowed={["Admin"]}><Employees/></RequireRole> </Layout>} />
                <Route path="/wip" element={<Layout> <WIP/> </Layout>} />
                <Route path="/inventory" element={<Layout> <Inventory /> </Layout>} />
                <Route path="/checkin" element={<Layout> <CheckIn /> </Layout>} />
                <Route path="/checkout" element={<Layout> <CheckOut /> </Layout>} />
                <Route path="/notifications" element={<Layout> <Notifications /> </Layout>} />
                <Route path="/profile" element={<Layout> <Profile /> </Layout>} />
                <Route path="/racks" element={<Layout> <RequireRole allowed={["Admin","Supervisor"]}><Racks /></RequireRole> </Layout>} />
                <Route path="/reports" element={<Layout> <RequireRole allowed={["Admin"]}><Reports /></RequireRole> </Layout>} />
                <Route path="/prediction" element={<Layout> <RequireRole allowed={["Admin"]}><Predictions /></RequireRole> </Layout>} />
                <Route path="/unauthorized" element={<Layout><Unauthorized /></Layout>} />
                <Route path="/racks/:rackCode" element={<RackProducts />} />

                <Route path="/reports/product-movement" element={<Layout><RequireRole allowed={["Admin"]}><ProductMovementReport /></RequireRole></Layout>} />
                <Route path="/reports/employee-activity" element={<Layout><RequireRole allowed={["Admin"]}><EmployeeActivityReport /></RequireRole></Layout>} />
                <Route path="/reports/warehouse" element={<Layout><RequireRole allowed={["Admin"]}><WarehouseReport /></RequireRole></Layout>} />
                <Route path="/reports/rack-utilization" element={<Layout><RequireRole allowed={["Admin"]}><RackUtilizationReport /></RequireRole></Layout>} />

                    </Routes>
              
        </BrowserRouter>
    );
}
export default App;

