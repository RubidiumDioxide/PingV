# --- Build Stage ---
FROM node:24-alpine AS builder

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN npm run build -- --configuration development 

# --- Runtime Stage ---
FROM nginx:alpine 

COPY --from=builder /app/dist/PingV.Client/browser /usr/share/nginx/html 

COPY nginx/default.conf /etc/nginx/conf.d/default.conf

EXPOSE 80 443 

CMD ["nginx", "-g", "daemon off;"]