# Build 
FROM node:alpine AS build
WORKDIR /app

COPY Clients/AngularSPA/package.json Clients/AngularSPA/package-lock.json ./
RUN npm ci

COPY Clients/AngularSPA/ ./
RUN npm run build -- --configuration=development

# Run 
FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/dist/AngularSPA/browser /usr/share/nginx/html

COPY Clients/AngularSPA/nginx/default.conf /etc/nginx/conf.d/default.conf

EXPOSE 80 443
CMD ["nginx", "-g", "daemon off;"]