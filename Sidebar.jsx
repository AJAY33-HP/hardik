import { NavLink } from "react-router-dom";
import "./Sidebar.css";
import { getCurrentUser, getAccessFromJWT } from "../utils/auth";
import {
    FaTachometerAlt,
    FaBoxOpen,
    FaUsers,
    FaWarehouse,
    FaClipboardList,
    FaSignInAlt,
    FaSignOutAlt,
    FaLayerGroup,
    FaFileAlt,
    FaBell,
    FaChartLine
} from "react-icons/fa";

function Sidebar() {
    // Get current user information including access permissions
    const currentUser = getCurrentUser() || {};
    const { role = null } = currentUser;

    // Get access permissions from localStorage or JWT
    const userAccess = getAccessFromJWT() || {};

    // Debug logging
    const rawAccessString = localStorage.getItem('access');
    console.log('Sidebar: access from storage:', rawAccessString);
    console.log('Sidebar: parsed access:', userAccess);

    const isAdmin = role === 'Admin';
    const isSupervisor = role === 'Supervisor';

    // Render a single menu item if permission is granted
    const renderMenuItem = (path, icon, label, permission) => {
        if (!permission) return null;

        return (
            <li className="sidebar-menu-item" key={path}>
                <NavLink
                    to={path}
                    end
                    className={({ isActive }) =>
                        `nav-link ${isActive ? "active" : ""}`
                    }
                    style={({ isActive }) => ({
                        background: isActive ? "#2563eb" : "#2d3f54",
                        color: "#ffffff",
                        textDecoration: "none",
                        display: "flex",
                        alignItems: "center",
                        padding: "12px 14px",
                        marginBottom: "6px",
                        borderRadius: "8px",
                        transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
                        border: isActive ? "1px solid #60a5fa" : "1px solid transparent",
                        boxShadow: isActive ? "0 4px 12px rgba(37, 99, 235, 0.4)" : "none"
                    })}
                >
                    <span style={{ 
                        marginRight: "12px", 
                        display: "flex", 
                        alignItems: "center",
                        fontSize: "18px"
                    }}>
                        {icon}
                    </span>
                    <span style={{ 
                        fontSize: "15px",
                        fontWeight: "500"
                    }}>
                        {label}
                    </span>
                </NavLink>
            </li>
        );
    };

    return (
        <div
            className="bg-dark text-white shadow-lg"
            style={{
                width: "270px",
                minHeight: "100vh",
                background: "linear-gradient(180deg, #1a1f2e 0%, #141820 100%)",
                overflowY: "auto",
                position: "sticky",
                top: "0",
                padding: "0",
                boxShadow: "2px 0 8px rgba(0, 0, 0, 0.3)"
            }}
        >
            {/* Sidebar Header */}
            <div style={{ 
                padding: "20px 16px", 
                borderBottom: "1px solid rgba(255, 255, 255, 0.1)",
                background: "rgba(0, 0, 0, 0.2)"
            }}>
                <h2 className="fw-bold" style={{ 
                    marginBottom: "4px", 
                    fontSize: "22px",
                    color: "#ffffff",
                    letterSpacing: "-0.5px"
                }}>WIP</h2>
                <p className="text-secondary" style={{ 
                    fontSize: "11px", 
                    marginBottom: "0",
                    color: "#9ca3af",
                    fontWeight: "600",
                    textTransform: "uppercase",
                    letterSpacing: "0.5px"
                }}>
                    Management
                </p>
            </div>

            {/* Navigation Menu */}
            <nav style={{ padding: "16px" }}>
                <ul className="nav flex-column" style={{ listStyle: "none", paddingLeft: "0", margin: "0" }}>
                    {/* Dashboard - Always available if access.dashboard is true */}
                    {userAccess.dashboard && renderMenuItem(
                        "/dashboard",
                        <FaTachometerAlt />,
                        "Dashboard",
                        true
                    )}

                    {/* Inventory */}
                    {userAccess.inventory && renderMenuItem(
                        "/inventory",
                        <FaClipboardList />,
                        "Inventory",
                        true
                    )}

                    {/* Check-In */}
                    {userAccess.checkIn && renderMenuItem(
                        "/checkIn",
                        <FaSignInAlt />,
                        "Check-In",
                        true
                    )}

                    {/* Check-Out */}
                    {userAccess.checkOut && renderMenuItem(
                        "/checkOut",
                        <FaSignOutAlt />,
                        "Check-Out",
                        true
                    )}

                    {/* Notifications */}
                    {userAccess.notifications && renderMenuItem(
                        "/notifications",
                        <FaBell />,
                        "Notifications",
                        true
                    )}

                    {/* Reports - Admin only + access permission */}
                    {userAccess.reports && isAdmin && renderMenuItem(
                        "/reports",
                        <FaFileAlt />,
                        "Reports",
                        true
                    )}

                    {/* Prediction - Admin only + access permission */}
                    {userAccess.prediction && isAdmin && renderMenuItem(
                        "/prediction",
                        <FaChartLine />,
                        "Prediction",
                        true
                    )}

                    {/* Divider */}
                    {(userAccess.products || userAccess.employees || userAccess.wip || userAccess.racks) && (
                        <li style={{ 
                            margin: "12px 0", 
                            borderTop: "1px solid rgba(255, 255, 255, 0.1)"
                        }}></li>
                    )}

                    {/* Products */}
                    {userAccess.products && renderMenuItem(
                        "/products",
                        <FaBoxOpen />,
                        "Products",
                        true
                    )}

                    {/* Employees - Admin only + access permission */}
                    {userAccess.employees && isAdmin && renderMenuItem(
                        "/employees",
                        <FaUsers />,
                        "Employees",
                        true
                    )}

                    {/* WIP */}
                    {userAccess.wip && renderMenuItem(
                        "/wip",
                        <FaWarehouse />,
                        "WIP",
                        true
                    )}

                    {/* Racks - Admin or Supervisor + access permission */}
                    {userAccess.racks && (isAdmin || isSupervisor) && renderMenuItem(
                        "/racks",
                        <FaLayerGroup />,
                        "Racks",
                        true
                    )}
                </ul>
            </nav>
        </div>
    );
}

export default Sidebar;
