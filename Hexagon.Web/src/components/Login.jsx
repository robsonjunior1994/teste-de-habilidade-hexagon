import React, { useState } from 'react';
import { authService } from '../services/api';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            const result = await authService.login(email, password);

            if (result.isSuccess) {
                localStorage.setItem('token', result.data);
                localStorage.setItem('userEmail', email);
                alert('Login successful!');
                window.location.href = '/dashboard';
            } else {
                setError(result.errorMessage || 'Login failed');
            }
        } catch (error) {
            setError(error.errorMessage || error.message || 'An error occurred');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="login-container">
            <div className="login-card">
                <div className="login-header">
                    <h2>Welcome Back</h2>
                    <p>Sign in to your account</p>
                </div>

                <form onSubmit={handleSubmit} className="login-form">
                    <div className="form-group">
                        <label htmlFor="email">Email Address</label>
                        <input
                            id="email"
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            disabled={loading}
                            placeholder="Enter your email"
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="password">Password</label>
                        <input
                            id="password"
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                            disabled={loading}
                            placeholder="Enter your password"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={loading}
                        className="login-button"
                    >
                        {loading ? (
                            <span className="loading-spinner">⏳</span>
                        ) : (
                            'Sign In'
                        )}
                    </button>
                </form>

                {error && (
                    <div className="error-message">
                        ⚠️ {error}
                    </div>
                )}

                <div className="login-footer">
                    <p>Don't have an account? <a href="/register">Sign up</a></p>
                </div>
            </div>

            <style jsx>{`
                .login-container {
                    width: 100%;
                    min-height: 100vh;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    padding: 20px;
                }

                .login-card {
                    background: white;
                    padding: 2.5rem;
                    border-radius: 16px;
                    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
                    width: 100%;
                    max-width: 400px;
                    margin: 0 auto;
                }

                .login-header {
                    text-align: center;
                    margin-bottom: 2rem;
                }

                .login-header h2 {
                    color: #333;
                    font-size: 1.8rem;
                    font-weight: 600;
                    margin-bottom: 0.5rem;
                }

                .login-header p {
                    color: #666;
                    font-size: 0.9rem;
                }

                .login-form {
                    margin-bottom: 1.5rem;
                }

                .form-group {
                    margin-bottom: 1.5rem;
                }

                .form-group label {
                    display: block;
                    margin-bottom: 0.5rem;
                    color: #333;
                    font-weight: 500;
                    font-size: 0.9rem;
                }

                .form-group input {
                    width: 100%;
                    padding: 0.75rem 1rem;
                    border: 2px solid #e1e5e9;
                    border-radius: 8px;
                    font-size: 1rem;
                    transition: all 0.3s ease;
                    background: #f8f9fa;
                }

                .form-group input:focus {
                    outline: none;
                    border-color: #667eea;
                    background: white;
                    box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
                }

                .form-group input:disabled {
                    background-color: #f5f5f5;
                    cursor: not-allowed;
                    opacity: 0.7;
                }

                .login-button {
                    width: 100%;
                    padding: 0.75rem;
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    color: white;
                    border: none;
                    border-radius: 8px;
                    font-size: 1rem;
                    font-weight: 600;
                    cursor: pointer;
                    transition: all 0.3s ease;
                    height: 48px;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                }

                .login-button:hover:not(:disabled) {
                    transform: translateY(-2px);
                    box-shadow: 0 10px 20px rgba(102, 126, 234, 0.3);
                }

                .login-button:disabled {
                    opacity: 0.7;
                    cursor: not-allowed;
                    transform: none;
                }

                .loading-spinner {
                    animation: spin 1s linear infinite;
                }

                @keyframes spin {
                    from { transform: rotate(0deg); }
                    to { transform: rotate(360deg); }
                }

                .error-message {
                    background: #fee;
                    color: #c53030;
                    padding: 0.75rem;
                    border-radius: 8px;
                    border: 1px solid #feb2b2;
                    margin-bottom: 1rem;
                    text-align: center;
                }

                .login-footer {
                    text-align: center;
                    padding-top: 1rem;
                    border-top: 1px solid #e1e5e9;
                }

                .login-footer p {
                    color: #666;
                    font-size: 0.9rem;
                }

                .login-footer a {
                    color: #667eea;
                    text-decoration: none;
                    font-weight: 500;
                }

                .login-footer a:hover {
                    text-decoration: underline;
                }

                /* Responsividade */
                @media (max-width: 480px) {
                    .login-container {
                        padding: 10px;
                    }

                    .login-card {
                        padding: 1.5rem;
                        margin: 0;
                    }
                    
                    .login-header h2 {
                        font-size: 1.5rem;
                    }

                    .login-header p {
                        font-size: 0.8rem;
                    }
                }

                @media (max-width: 768px) {
                    .login-card {
                        max-width: 90%;
                    }
                }

                /* Animações */
                .login-card {
                    animation: fadeInUp 0.6s ease-out;
                }

                @keyframes fadeInUp {
                    from {
                        opacity: 0;
                        transform: translateY(30px);
                    }
                    to {
                        opacity: 1;
                        transform: translateY(0);
                    }
                }
            `}</style>
        </div>
    );
};

export default Login;