import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api, { jwtService } from '../services/api';

const Customers = () => {
    const [customers, setCustomers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [deletingId, setDeletingId] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        checkAuthAndFetchCustomers();
    }, [navigate]);

    const checkAuthAndFetchCustomers = async () => {
        const token = localStorage.getItem('token');

        if (!token) {
            navigate('/login');
            return;
        }

        try {
            const userId = jwtService.getUserIdFromToken(token);
            if (!userId) {
                throw new Error('Invalid token');
            }

            await fetchCustomers();
        } catch (error) {
            console.error('Auth error:', error);
            localStorage.removeItem('token');
            navigate('/login');
        } finally {
            setLoading(false);
        }
    };

    const fetchCustomers = async () => {
        try {
            const response = await api.get('/customer');

            if (response.data.isSuccess) {
                setCustomers(response.data.data || []);
            } else {
                setError(response.data.errorMessage || 'Failed to fetch customers');
            }
        } catch (error) {
            console.error('Error fetching customers:', error);
            setError(error.response?.data?.errorMessage || error.message || 'Error loading customers');
        }
    };

    const handleDeleteCustomer = async (customerId) => {
        if (!window.confirm('Are you sure you want to delete this customer?')) {
            return;
        }

        setDeletingId(customerId);

        try {
            const response = await api.delete(`/customer/${customerId}`);

            if (response.data.isSuccess) {
                // Remove o customer da lista localmente
                setCustomers(customers.filter(customer => customer.id !== customerId));
                alert('Customer deleted successfully!');

                // Recarrega a lista para garantir sincronização
                await fetchCustomers();
            } else {
                setError(response.data.errorMessage || 'Failed to delete customer');
            }
        } catch (error) {
            console.error('Error deleting customer:', error);
            setError(error.response?.data?.errorMessage || error.message || 'Error deleting customer');
        } finally {
            setDeletingId(null);
        }
    };

    const handleBack = () => {
        navigate('/home');
    };

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-gray-100">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto"></div>
                    <p className="mt-4 text-gray-600">Loading customers...</p>
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
                        <div className="flex items-center">
                            <button
                                onClick={handleBack}
                                className="bg-gray-500 hover:bg-gray-600 text-white px-4 py-2 rounded-lg mr-4 transition-colors duration-200"
                            >
                                ← Back
                            </button>
                            <h1 className="text-2xl font-bold text-gray-800">Customers</h1>
                        </div>
                        <div className="flex space-x-4">
                            <button
                                onClick={() => navigate('/customers/add')}
                                className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-lg transition-colors duration-200"
                            >
                                + Add Customer
                            </button>
                            <button
                                onClick={() => {
                                    localStorage.removeItem('token');
                                    navigate('/login');
                                }}
                                className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-lg transition-colors duration-200"
                            >
                                Logout
                            </button>
                        </div>
                    </div>
                </div>
            </header>

            {/* Main Content */}
            <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                <div className="bg-white rounded-2xl shadow-2xl p-8">
                    {error && (
                        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6">
                            ⚠️ {error}
                            <button
                                onClick={() => setError('')}
                                className="ml-4 text-red-800 hover:text-red-900"
                            >
                                ×
                            </button>
                        </div>
                    )}

                    <div className="mb-6">
                        <h2 className="text-2xl font-bold text-gray-800 mb-2">
                            Customer List ({customers.length})
                        </h2>
                        <p className="text-gray-600">All registered customers in the system</p>
                    </div>

                    {customers.length === 0 ? (
                        <div className="text-center py-12">
                            <p className="text-gray-500 text-lg">No customers found</p>
                            <button
                                onClick={fetchCustomers}
                                className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-lg mt-4"
                            >
                                Refresh
                            </button>
                        </div>
                    ) : (
                        <>
                            <div className="overflow-x-auto">
                                <table className="min-w-full bg-white border border-gray-200 rounded-lg">
                                    <thead>
                                        <tr className="bg-gray-50">
                                            <th className="px-6 py-3 border-b border-gray-200 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                                ID
                                            </th>
                                            <th className="px-6 py-3 border-b border-gray-200 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                                Name
                                            </th>
                                            <th className="px-6 py-3 border-b border-gray-200 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                                Age
                                            </th>
                                            <th className="px-6 py-3 border-b border-gray-200 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                                Civil State
                                            </th>
                                            <th className="px-6 py-3 border-b border-gray-200 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                                CPF
                                            </th>
                                            <th className="px-6 py-3 border-b border-gray-200 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                                City
                                            </th>
                                            <th className="px-6 py-3 border-b border-gray-200 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                                State
                                            </th>
                                            <th className="px-6 py-3 border-b border-gray-200 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                                Actions
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody className="divide-y divide-gray-200">
                                        {customers.map((customer) => (
                                            <tr key={customer.id} className="hover:bg-gray-50">
                                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                                                    {customer.id}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                                    {customer.name}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                                    {customer.age}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 capitalize">
                                                    {customer.civilState}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-mono">
                                                    {customer.cpf}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                                    {customer.city}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                                    {customer.state}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                                    <div className="flex space-x-2">
                                                        <button
                                                            onClick={() => navigate(`/customers/edit/${customer.id}`)}
                                                            className="bg-blue-500 hover:bg-blue-600 text-white px-3 py-1 rounded text-xs transition-colors duration-200"
                                                        >
                                                            Edit
                                                        </button>
                                                        <button
                                                            onClick={() => handleDeleteCustomer(customer.id)}
                                                            disabled={deletingId === customer.id}
                                                            className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded text-xs disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-200"
                                                        >
                                                            {deletingId === customer.id ? 'Deleting...' : 'Delete'}
                                                        </button>
                                                    </div>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>

                            {/* Cards view for mobile */}
                            <div className="mt-6 grid grid-cols-1 gap-4 md:hidden">
                                {customers.map((customer) => (
                                    <div key={customer.id} className="bg-gray-50 p-4 rounded-lg border border-gray-200">
                                        <div className="flex justify-between items-start mb-2">
                                            <span className="text-sm font-medium text-gray-900">#{customer.id}</span>
                                            <span className="text-sm text-gray-500">{customer.cpf}</span>
                                        </div>
                                        <h3 className="text-lg font-semibold text-gray-800 mb-2">{customer.name}</h3>
                                        <div className="grid grid-cols-2 gap-2 text-sm mb-3">
                                            <div>
                                                <span className="text-gray-600">Age:</span> {customer.age}
                                            </div>
                                            <div>
                                                <span className="text-gray-600">Civil State:</span> {customer.civilState}
                                            </div>
                                            <div>
                                                <span className="text-gray-600">City:</span> {customer.city}
                                            </div>
                                            <div>
                                                <span className="text-gray-600">State:</span> {customer.state}
                                            </div>
                                        </div>
                                        {/* ↓↓↓ BOTÕES EDIT E DELETE PARA MOBILE ↓↓↓ */}
                                        <div className="flex space-x-2 mt-3">
                                            <button
                                                onClick={() => navigate(`/customers/edit/${customer.id}`)}
                                                className="bg-blue-500 hover:bg-blue-600 text-white px-3 py-1 rounded text-xs transition-colors duration-200 flex-1"
                                            >
                                                Edit
                                            </button>
                                            <button
                                                onClick={() => handleDeleteCustomer(customer.id)}
                                                disabled={deletingId === customer.id}
                                                className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded text-xs disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-200 flex-1"
                                            >
                                                {deletingId === customer.id ? 'Deleting...' : 'Delete'}
                                            </button>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </>
                    )}
                </div>
            </main>
        </div>
    );
};

export default Customers;