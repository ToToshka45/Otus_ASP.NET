# Сборка фронта
FROM node:20 AS build
ENV DOCKER=true
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# Сервер для отдачи статичных файлов
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80