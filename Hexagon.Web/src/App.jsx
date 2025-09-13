import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import Home from './components/Home';
import Customers from './components/Customers'; 
import ProtectedRoute from './components/ProtectedRoute';
import AddCustomer from './components/AddCustomer'; 
import EditCustomer from './components/EditCustomer';
import './index.css';

function App() {
    return (
        <div className="app-container">
            <Routes>
                {/* Rotas públicas */}
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />

                {/* Rotas protegidas */}
                <Route path="/home" element={
                    <ProtectedRoute>
                        <Home />
                    </ProtectedRoute>
                } />

                <Route path="/customers" element={
                    <ProtectedRoute>
                        <Customers />
                    </ProtectedRoute>
                } />

                <Route path="/customers/add" element={
                    <ProtectedRoute>
                        <AddCustomer />
                    </ProtectedRoute>
                } />

                <Route path="/customers/edit/:id" element={
                    <ProtectedRoute>
                        <EditCustomer />
                    </ProtectedRoute>
                } />

                {/* Redirecionamentos */}
                <Route path="/" element={<Navigate to="/login" replace />} />
                <Route path="*" element={<div>Page not found</div>} />
            </Routes>
        </div>
    );
}

export default App;