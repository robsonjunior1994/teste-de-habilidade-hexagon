import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 63218,
        proxy: {
            '/api': {
                target: 'https://localhost:7055',  // ← HTTPS e porta 7055
                changeOrigin: true,
                secure: false,  // ← false para certificado auto-assinado
                rewrite: (path) => path.replace(/^\/api/, '/api')
            }
        }
    }
});