import axios from 'axios';

const api = axios.create({
    baseURL: '/api',
    headers: {
        'Content-Type': 'application/json',
    },
});

// Interceptor para adicionar token automaticamente
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// ✅ REMOVA o export daqui - deixe apenas const
const jwtService = {
    decodeToken: (token) => {
        try {
            const payload = token.split('.')[1];
            return JSON.parse(atob(payload));
        } catch {
            return null;
        }
    },

    getUserIdFromToken: (token) => {
        const decoded = jwtService.decodeToken(token);
        return decoded?.nameid || decoded?.NameIdentifier || null;
    },

    getUserEmailFromToken: (token) => {
        const decoded = jwtService.decodeToken(token);
        return decoded?.email || decoded?.Email || null;
    },

    getUserNameFromToken: (token) => {
        const decoded = jwtService.decodeToken(token);
        return decoded?.unique_name || decoded?.UniqueName || decoded?.name || null;
    },

    isTokenExpired: (token) => {
        const decoded = jwtService.decodeToken(token);
        if (!decoded?.exp) return true;
        return Date.now() >= decoded.exp * 1000;
    }
};

const authService = {
    login: async (email, password) => {
        try {
            const response = await api.post('/user/login', { email, password });

            if (response.data.isSuccess && response.data.data?.token) {
                localStorage.setItem('token', response.data.data.token);
                localStorage.setItem('userEmail', email);
            }

            return response.data;
        } catch (error) {
            throw error.response?.data || error.message;
        }
    },
};

const userService = {
    register: async (userData) => {
        try {
            const response = await api.post('/user', userData);
            return response.data;
        } catch (error) {
            throw error.response?.data || error.message;
        }
    },
};

// Adicione no customerService:
export const customerService = {
    create: async (customerData) => {
        try {
            const response = await api.post('/customer', customerData);
            return response.data;
        } catch (error) {
            throw error.response?.data || error.message;
        }
    },

    getById: async (customerId) => {
        try {
            const response = await api.get(`/customer/${customerId}`);
            return response.data;
        } catch (error) {
            throw error.response?.data || error.message;
        }
    },

    update: async (customerId, customerData) => {
        try {
            const response = await api.put(`/customer/${customerId}`, customerData);
            return response.data;
        } catch (error) {
            throw error.response?.data || error.message;
        }
    },

    delete: async (customerId) => {
        try {
            const response = await api.delete(`/customer/${customerId}`);
            return response.data;
        } catch (error) {
            throw error.response?.data || error.message;
        }
    }
};

// ✅ APENAS UMA exportação final
export { authService, userService, jwtService };
export default api;