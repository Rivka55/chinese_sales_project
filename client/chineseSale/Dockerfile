# Use official Node image as a build stage
FROM node:20 AS build

# Set working directory
WORKDIR /app

# Copy package files and install dependencies
COPY package.json package-lock.json* ./
RUN npm ci

# Copy the rest of the application and build
COPY . .
RUN npm run build

# Stage for serving content with nginx
FROM nginx:stable-alpine AS final

# Copy built files to nginx html folder
COPY --from=build /app/dist/chineseSale/* /usr/share/nginx/html/

# Expose port
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]