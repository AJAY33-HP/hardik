// ============================================================
// ProtectedRoute.jsx - Permission-Based Route Protection
// ============================================================
// Protects routes based on PERMISSIONS (not roles)
// Permissions are stored in localStorage as comma-separated string
// Example: localStorage.access = "dashboard,products,wip,inventory"
//
// Usage in App.jsx:
// <Route path="/dashboard" element={<ProtectedRoute requiredPermission="dashboard"><Dashboard /></ProtectedRoute>} />
// <Route path="/employees" element={<ProtectedRoute requiredPermission="employees"><Employees /></ProtectedRoute>} />

import { Navigate } from 'react-router-dom';
import { hasPermission } from '../utils/auth';

/**
 * ProtectedRoute Component
 * @param {string} requiredPermission - The permission needed to access this route (e.g., 'dashboard', 'employees')
 * @param {ReactNode} children - The component to render if permission is granted
 * @returns {ReactNode} - Either renders children or redirects to /unauthorized
 */
function ProtectedRoute({ requiredPermission, children }) {
    // Get token to check if user is logged in
    const token = localStorage.getItem('token');

    // If no token, user is not logged in - redirect to login
    if (!token) {
        console.warn('ProtectedRoute: No token found, redirecting to login');
        return <Navigate to="/login" replace />;
    }

    // Check if user has required permission
    // hasPermission reads from latest localStorage.access value
    const hasAccess = hasPermission(requiredPermission);

    if (!hasAccess) {
        console.warn(`ProtectedRoute: User does not have permission '${requiredPermission}', redirecting to /unauthorized`);
        return <Navigate to="/unauthorized" replace />;
    }

    // User has permission, render the component
    console.log(`ProtectedRoute: User granted access to '${requiredPermission}'`);
    return children;
}

export default ProtectedRoute;
