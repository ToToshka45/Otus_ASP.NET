import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

// Флаг: запускаемся ли мы внутри Docker?
const isDocker = env.DOCKER === 'true';
// Флаг: режим разработки?
const isDev = env.NODE_ENV === 'development';

// Сертификаты — только если не в Docker
let httpsOptions = false;

if (isDev && !isDocker) {
    const baseFolder =
        env.APPDATA && env.APPDATA !== ''
            ? `${env.APPDATA}/ASP.NET/https`
            : `${env.HOME}/.aspnet/https`;

    const certificateName = "twohostsreactapp.client";
    const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
    const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        const result = child_process.spawnSync('dotnet', [
            'dev-certs', 'https',
            '--export-path', certFilePath,
            '--format', 'Pem',
            '--no-password',
        ], { stdio: 'inherit' });

        if (result.status !== 0) {
            throw new Error("Could not create certificate.");
        }
    }

    httpsOptions = {
        key: fs.readFileSync(keyFilePath),
        cert: fs.readFileSync(certFilePath),
    };
}

// Куда проксировать запросы с фронта
const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
        ? env.ASPNETCORE_URLS.split(';')[0]
        : isDocker
            ? 'http://backend:8090' // имя сервиса в docker-compose
            : 'https://localhost:7243'; // fallback локальный адрес

export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/weatherforecast': {
                target,
                changeOrigin: true,
                secure: false
            }
        },
        port: 5173,
        https: httpsOptions
    }
});