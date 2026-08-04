// ============================================================
// Login.jsx - User Authentication
// ============================================================
// Handles user login and saves all required data to localStorage:
// - token: JWT authentication token
// - role: User role (Admin, Supervisor, etc.)
// - employeeId: Employee ID from response
// - employeeCode: Employee code from response
// - name: Employee name from response
// - access: Access permissions from JWT claim (comma-separated string)

import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import { parseJwt } from "../utils/auth";

function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setError("");
        setIsLoading(true);

        try {
            // Call login API endpoint
            const response = await axios.post("https://localhost:44321/api/login", {
                email,
                password,
            });

            // Extract all data from response
            const { token, role, employeeId, employeeCode, name } = response.data;

            // Look for access in multiple places
            let access = response.data.access || response.data.Access || null;

            // Save all data to localStorage for later access
            localStorage.setItem("token", token);
            localStorage.setItem("role", role);
            localStorage.setItem("employeeId", employeeId || "");
            localStorage.setItem("employeeCode", employeeCode || "");
            localStorage.setItem("name", name || "");

            // Extract access permissions from response OR JWT token
            // Priority: 1) response.data.access, 2) JWT claim 'access'
            let accessString = access;
            let accessSource = 'response.data';

            if (!accessString) {
                // If access not in response body, extract from JWT token
                // The JWT token contains a claim called 'access' with comma-separated permissions
                const claims = parseJwt(token);
                console.log("Login: JWT claims:", claims);
                if (claims && (claims.access || claims.Access)) {
                    accessString = claims.access || claims.Access;
                    accessSource = 'JWT claim';
                    console.log("Login: Found access in JWT claim:", accessString);
                }
            }

            // Clear old access before setting new one
            localStorage.removeItem("access");

            // Save access string (e.g., "dashboard,products,wip,inventory")
            if (accessString) {
                localStorage.setItem("access", accessString);
                console.log(`Login: Access stored from ${accessSource}:`, accessString);
            } else {
                // If no access found, store empty string (sidebar will show nothing)
                localStorage.setItem("access", "");
                console.warn('Login: No access permissions found in response or JWT');
            }

            // Debug: Log all stored values and full response
            console.log("Login successful. Response data:", response.data);
            console.log("Login: Stored values:", {
                token: token ? `${token.substring(0, 20)}...` : null,
                role,
                employeeId,
                employeeCode,
                name,
                access: accessString || "(empty)",
                accessSource
            });
            alert("Login successful!");

            // Navigate to dashboard
            navigate("/dashboard");
        } catch (error) {
            console.error("Login error:", error);
            const errorMessage = error.response?.data?.message || "Invalid Email or Password";
            setError(errorMessage);
            alert(errorMessage);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="login page d-flex justify-content-center align-items-center bg-light py-5">
            <div className="card shadow p-4" style={{ maxWidth: 420, width: '100%' }}>
                <div className="text-center mb-3">
                    <h2 className="mb-1">WIP Management</h2>
                    <p className="text-center text-muted mb-0">Sign in to your account</p>
                </div>

                {error && (
                    <div className="alert alert-danger alert-dismissible fade show" role="alert">
                        {error}
                        <button type="button" className="btn-close" onClick={() => setError("")}></button>
                    </div>
                )}

                <form onSubmit={handleLogin}>
                    <div className="mb-3">
                        <label className="form-label">Employee ID / Email</label>
                        <input
                            type="email"
                            className="form-control"
                            placeholder="Enter email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            disabled={isLoading}
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Password</label>
                        <input
                            type="password"
                            className="form-control"
                            placeholder="Enter password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            disabled={isLoading}
                            required
                        />
                    </div>
                    <button 
                        type="submit" 
                        className="btn btn-primary w-100"
                        disabled={isLoading}
                    >
                        {isLoading ? "Logging in..." : "Login"}
                    </button>
                </form>

                <div className="text-center mt-3 small">
                    New user? <Link to="/register">Register here</Link>
                </div>
            </div>
        </div>
    );
}

export default Login;
