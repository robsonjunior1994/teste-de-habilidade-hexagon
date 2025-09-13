import React from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import Login from './components/Login'
import './App.css'

function App() {
    return (
        <div className="app-container">
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<div>P�gina de Registro - Em Desenvolvimento</div>} />
                <Route path="/" element={<Navigate to="/login" replace />} />
                <Route path="*" element={<div>P�gina n�o encontrada</div>} />
            </Routes>
        </div>
    )
}

export default App