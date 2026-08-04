// ============================================================
// auth.js - Authentication and Permission Management
// ============================================================
// This file handles JWT parsing, permission management,
// and user information retrieval from localStorage and JWT tokens.

// Parse JWT token and return the payload claims
export function parseJwt(token) {
    if (!token) return null;
    try {
        const parts = token.split('.');
        if (parts.length < 2) return null;
        const payload = parts[1];
        const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
        return JSON.parse(decodeURIComponent(escape(decoded)));
    } catch (e) {
        console.error('JWT Parse Error:', e);
        try {
            // Fallback without decodeURIComponent for environments that fail
            const parts = token.split('.');
            const payload = parts[1];
            const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
            return JSON.parse(decoded);
        } catch (err) {
            console.error('JWT Parse Fallback Error:', err);
            return null;
        }
    }
}

// Convert comma-separated access string to object with boolean flags
// Example: "dashboard,products,wip" → { dashboard: true, products: true, wip: true, ... }
export function parseAccessString(accessString) {
    // Default structure with all permissions false
    // Only permissions explicitly listed in accessString will be true
    const defaultAccess = {
        dashboard: false,
        products: false,
        employees: false,
        wip: false,
        inventory: false,
        checkIn: false,
        checkOut: false,
        reports: false,
        racks: false,
        notifications: false,
        prediction: false,
    };

    // If no access string provided, return all false (no access)
    if (!accessString || accessString.trim() === '') {
        console.log('parseAccessString: No access string provided, all permissions set to false');
        return defaultAccess;
    }

    // Split and normalize each permission
    const accessArray = accessString.split(',').map(item => item.trim().toLowerCase());

    console.log('parseAccessString: Raw access string:', accessString);
    console.log('parseAccessString: Parsed array:', accessArray);

    // Build result with permission checks
    const result = {
        dashboard: accessArray.includes('dashboard'),
        products: accessArray.includes('products'),
        employees: accessArray.includes('employees'),
        wip: accessArray.includes('wip'),
        inventory: accessArray.includes('inventory'),
        checkIn: accessArray.includes('checkin') || accessArray.includes('check-in') || accessArray.includes('check_in'),
        checkOut: accessArray.includes('checkout') || accessArray.includes('check-out') || accessArray.includes('check_out'),
        reports: accessArray.includes('reports'),
        racks: accessArray.includes('racks'),
        notifications: accessArray.includes('notifications'),
        prediction: accessArray.includes('prediction'),
    };

    console.log('parseAccessString: Returned result:', result);
    return result;
}

// Convert access object back to comma-separated string
// Example: { dashboard: true, products: true } → "dashboard,products"
export function accessObjectToString(accessObj) {
    const permissions = [];
    Object.entries(accessObj).forEach(([key, value]) => {
        // Only include permissions that are true
        if (value) {
            permissions.push(key);
        }
    });
    return permissions.join(',');
}

// Extract access from JWT token or localStorage
// Reads 'access' from localStorage first, then tries JWT claims
export function getAccessFromJWT() {
    const token = localStorage.getItem('token');
    const accessFromStorage = localStorage.getItem('access');

    console.log('getAccessFromJWT: Checking for access...');
    console.log('getAccessFromJWT: accessFromStorage:', accessFromStorage);

    // If access is stored in localStorage, use it (updated by login)
    if (accessFromStorage && accessFromStorage.trim()) {
        console.log('getAccessFromJWT: Using access from localStorage');
        return parseAccessString(accessFromStorage);
    }

    // Otherwise, try to extract from JWT claims
    console.log('getAccessFromJWT: localStorage access empty, trying JWT claims');
    const claims = parseJwt(token) || {};
    const accessString = claims.access || claims.Access || '';
    console.log('getAccessFromJWT: JWT access claim:', accessString);
    return parseAccessString(accessString);
}

// Check if user has a specific permission
// Example: hasPermission('dashboard') returns true if user has dashboard access
export function hasPermission(permission) {
    if (!permission) return false;
    const access = getAccessFromJWT();
    return access[permission] === true;
}

// Get current user information from localStorage and JWT
// Returns: { role, name, id, employeeCode, employeeId, claims, access }
export function getCurrentUser() {
    const token = localStorage.getItem('token');
    const roleFromStorage = localStorage.getItem('role');
    const employeeCodeFromStorage = localStorage.getItem('employeeCode');
    const employeeIdFromStorage = localStorage.getItem('employeeId');
    const nameFromStorage = localStorage.getItem('name');

    // Parse JWT claims
    const claims = parseJwt(token) || {};

    // Extract role from localStorage or JWT
    const role = roleFromStorage || 
                 claims.role || 
                 claims.roles || 
                 claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 
                 null;

    // Extract user info from localStorage first (preferred), then JWT
    const name = nameFromStorage || 
                 claims.unique_name || 
                 claims.name || 
                 claims.given_name || 
                 claims.email || 
                 null;

    const id = claims.nameid || claims.sub || claims.id || null;

    // Get access permissions
    const access = getAccessFromJWT();

    // Return complete user object
    return { 
        role, 
        name, 
        id, 
        employeeCode: employeeCodeFromStorage,
        employeeId: employeeIdFromStorage,
        claims, 
        access 
    };
}
