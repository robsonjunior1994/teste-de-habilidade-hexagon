// ProtectedRoute.jsx melhorado
import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { jwtService } from '../services/api';

const ProtectedRoute = ({ children }) => {
    const [isValid, setIsValid] = useState(null);
    const token = localStorage.getItem('token');

    useEffect(() => {
        if (token) {
            const isExpired = jwtService.isTokenExpired(token);
            setIsValid(!isExpired);
        } else {
            setIsValid(false);
        }
    }, [token]);

    if (isValid === null) {
        // Loading enquanto verifica
        return (
            <div className="min-h-screen flex items-center justify-center">
                <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-500"></div>
            </div>
        );
    }

    return isValid ? children : <Navigate to="/login" replace />;
};

export default ProtectedRoute;