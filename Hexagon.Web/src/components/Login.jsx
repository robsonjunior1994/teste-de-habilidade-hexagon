import React, { useState } from 'react';
import { authService } from '../services/api';
import { useNavigate } from 'react-router-dom';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            const result = await authService.login(email, password);
            console.log('Login result:', result); // Debug

            if (result.isSuccess) {
                // ✅ TRATAMENTO SEGURO para diferentes estruturas de response
                const token = result.data?.data?.token || // estrutura: { data: { token: "..." } }
                    result.data?.token ||        // estrutura: { data: { token: "..." } } alternativa
                    result.token ||              // estrutura: { token: "..." }
                    result.data;                 // estrutura: { data: "token-string" }

                if (!token) {
                    console.error('Token not found in response:', result);
                    setError('Login successful but no token received');
                    return;
                }

                localStorage.setItem('token', token);
                localStorage.setItem('userEmail', email);
                navigate('/home');
            } else {
                setError(result.errorMessage || 'Login failed');
            }
        } catch (error) {
            console.error('Login error:', error);
            // ✅ TRATAMENTO SEGURO para erro
            const errorMessage = error.response?.data?.errorMessage ||
                error.errorMessage ||
                error.message ||
                'An error occurred during login';
            setError(errorMessage);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-500 to-purple-600 px-4">
            <div className="bg-white p-8 rounded-2xl shadow-2xl w-full max-w-md">
                <div className="text-center mb-8">
                    <h2 className="text-3xl font-bold text-gray-800">Welcome Back</h2>
                    <p className="text-gray-600 mt-2">Sign in to your account</p>
                </div>

                <form onSubmit={handleSubmit} className="space-y-6">
                    <div>
                        <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-2">
                            Email Address
                        </label>
                        <input
                            id="email"
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            disabled={loading}
                            placeholder="Enter your email"
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-gray-100 disabled:cursor-not-allowed"
                        />
                    </div>

                    <div>
                        <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-2">
                            Password
                        </label>
                        <input
                            id="password"
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                            disabled={loading}
                            placeholder="Enter your password"
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-gray-100 disabled:cursor-not-allowed"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full bg-gradient-to-r from-blue-500 to-purple-600 text-white py-3 px-4 rounded-lg font-semibold hover:from-blue-600 hover:to-purple-700 focus:ring-4 focus:ring-blue-300 disabled:opacity-70 disabled:cursor-not-allowed transition-all duration-200 transform hover:-translate-y-0.5"
                    >
                        {loading ? (
                            <div className="flex items-center justify-center">
                                <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></div>
                                <span className="ml-2">Loading...</span>
                            </div>
                        ) : (
                            'Sign In'
                        )}
                    </button>
                </form>

                {error && (
                    <div className="mt-6 bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
                        ⚠️ {error}
                    </div>
                )}

                <div className="mt-6 pt-6 border-t border-gray-200 text-center">
                    <p className="text-gray-600">
                        Don't have an account?{' '}
                        <a href="/register" className="text-blue-500 font-semibold hover:text-blue-700">
                            Sign up
                        </a>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default Login;