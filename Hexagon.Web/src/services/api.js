import axios from 'axios';

// Configuração base da API - use proxy do Vite
const api = axios.create({
    baseURL: '/api',  // ← Vai usar o proxy para https://localhost:7055
    headers: {
        'Content-Type': 'application/json',
    },
});

// Interceptor para debug (remova depois)
api.interceptors.request.use((config) => {
    console.log('🔄 Making request to:', config.url);
    return config;
});

api.interceptors.response.use(
    (response) => {
        console.log('✅ Response:', response.status, response.data);
        return response;
    },
    (error) => {
        console.log('❌ Error:', error.response?.status, error.response?.data);
        return Promise.reject(error);
    }
);

export const authService = {
    login: async (email, password) => {
        try {
            const response = await api.post('/user/login', {
                email,
                password
            });
            return response.data;
        } catch (error) {
            throw error.response?.data || error.message;
        }
    },
};

export default api;