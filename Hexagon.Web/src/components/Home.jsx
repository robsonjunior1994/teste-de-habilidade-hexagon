import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtService } from '../services/api';

const Home = () => {
    const [userData, setUserData] = useState(null);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        const checkAuth = async () => {
            const token = localStorage.getItem('token');

            if (!token) {
                navigate('/login');
                return;
            }

            try {
                // Verifica se o token é válido e extrai dados
                const userId = jwtService.getUserIdFromToken(token);
                const userEmail = jwtService.getUserEmailFromToken(token);
                const userName = jwtService.getUserNameFromToken(token);

                if (!userId) {
                    throw new Error('Invalid token');
                }

                setUserData({
                    id: userId,
                    email: userEmail,
                    name: userName
                });
            } catch (error) {
                console.error('Auth error:', error);
                localStorage.removeItem('token');
                localStorage.removeItem('userEmail');
                navigate('/login');
            } finally {
                setLoading(false);
            }
        };

        checkAuth();
    }, [navigate]);

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('userEmail');
        navigate('/login');
    };

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-gray-100">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto"></div>
                    <p className="mt-4 text-gray-600">Loading...</p>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gradient-to-br from-blue-50 to-purple-100">
            {/* Header */}
            <header className="bg-white shadow-sm">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
                    <div className="flex justify-between items-center">
                        <h1 className="text-2xl font-bold text-gray-800">Dashboard</h1>
                        <button
                            onClick={handleLogout}
                            className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-lg transition-colors duration-200"
                        >
                            Logout
                        </button>
                    </div>
                </div>
            </header>

            {/* Main Content */}
            <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                <div className="bg-white rounded-2xl shadow-2xl p-8">
                    <div className="text-center mb-8">
                        <div className="w-24 h-24 bg-gradient-to-r from-blue-500 to-purple-600 rounded-full flex items-center justify-center mx-auto mb-4">
                            <span className="text-white text-3xl font-bold">
                                {userData?.name?.charAt(0)?.toUpperCase() || 'U'}
                            </span>
                        </div>
                        <h2 className="text-3xl font-bold text-gray-800 mb-2">
                            Welcome back, {userData?.name || 'User'}!
                        </h2>
                        <p className="text-gray-600">{userData?.email}</p>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {/* Stats Cards */}
                        <div className="bg-blue-50 p-6 rounded-lg border border-blue-100">
                            <h3 className="text-lg font-semibold text-blue-800 mb-2">User ID</h3>
                            <p className="text-2xl font-bold text-blue-600">{userData?.id}</p>
                        </div>

                        <div className="bg-green-50 p-6 rounded-lg border border-green-100">
                            <h3 className="text-lg font-semibold text-green-800 mb-2">Status</h3>
                            <p className="text-2xl font-bold text-green-600">Active</p>
                        </div>

                        <div className="bg-purple-50 p-6 rounded-lg border border-purple-100">
                            <h3 className="text-lg font-semibold text-purple-800 mb-2">Session</h3>
                            <p className="text-2xl font-bold text-purple-600">Valid</p>
                        </div>
                    </div>



                    <div className="mt-8 text-center">

                        <button
                            onClick={() => navigate('/customers')}
                            className="bg-green-500 hover:bg-green-600 text-white px-6 py-3 rounded-lg font-semibold transition-colors duration-200 mx-2"
                        >
                            View Customers
                        </button>

                    </div>
                </div>
            </main>
        </div>
    );
};

export default Home;