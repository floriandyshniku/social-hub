# Docker Setup for newProject

This document explains how to containerize and run the newProject application using Docker.

## Prerequisites

- Docker Desktop installed on your machine
- Docker Compose (usually included with Docker Desktop)

## Quick Start

### 1. Build and Run with Docker Compose (Development)

```bash
# Navigate to the project directory
cd newProject

# Build and start all services
docker-compose up --build

# Or run in detached mode
docker-compose up --build -d
```

This will:
- Build the .NET application
- Start SQL Server database
- Run the application on http://localhost:8080
- Create a persistent volume for database data

### 2. Production Deployment

```bash
# Set your database connection string
export DATABASE_CONNECTION_STRING="your-production-connection-string"

# Run production configuration
docker-compose -f docker-compose.prod.yml up --build -d
```

## Docker Commands

### Build the Image
```bash
docker build -t newproject:latest .
```

### Run the Container
```bash
docker run -p 8080:8080 newproject:latest
```

### View Logs
```bash
# All services
docker-compose logs

# Specific service
docker-compose logs app
docker-compose logs sqlserver

# Follow logs in real-time
docker-compose logs -f app
```

### Stop Services
```bash
# Stop and remove containers
docker-compose down

# Stop and remove containers + volumes
docker-compose down -v

# Stop and remove containers + volumes + images
docker-compose down -v --rmi all
```

### Clean Up
```bash
# Remove all containers, networks, and images
docker system prune -a

# Remove specific images
docker rmi newproject:latest
```

## Environment Variables

### Development (docker-compose.yml)
- `ASPNETCORE_ENVIRONMENT=Development`
- `ConnectionStrings__DefaultConnection` - Points to local SQL Server

### Production (docker-compose.prod.yml)
- `ASPNETCORE_ENVIRONMENT=Production`
- `DATABASE_CONNECTION_STRING` - External database connection

## Database Setup

The application will automatically create the database and run migrations when it starts. The SQL Server container includes:

- **Port**: 1433
- **Username**: sa
- **Password**: YourStrong@Passw0rd
- **Database**: SocialHubDb (created automatically)

## Health Checks

The application includes health check endpoints:
- **Application**: http://localhost:8080/health
- **Docker Health Check**: Configured in Dockerfile

## Troubleshooting

### Common Issues

1. **Port Already in Use**
   ```bash
   # Check what's using the port
   netstat -ano | findstr :8080
   
   # Change port in docker-compose.yml
   ports:
     - "8081:8080"  # Use different host port
   ```

2. **Database Connection Issues**
   ```bash
   # Check if SQL Server is running
   docker-compose logs sqlserver
   
   # Wait for database to be ready
   docker-compose up sqlserver
   ```

3. **Build Failures**
   ```bash
   # Clean build cache
   docker builder prune
   
   # Rebuild without cache
   docker-compose build --no-cache
   ```

### Logs and Debugging

```bash
# View application logs
docker-compose logs app

# Access container shell
docker-compose exec app sh

# Check container status
docker-compose ps
```

## Security Considerations

1. **Change Default Passwords**: Update the SQL Server password in production
2. **Use Environment Variables**: Store sensitive data in environment variables
3. **Network Security**: Use Docker networks to isolate services
4. **Non-Root User**: The application runs as a non-root user for security

## Performance Optimization

1. **Multi-Stage Build**: The Dockerfile uses multi-stage builds to reduce image size
2. **Layer Caching**: Dependencies are installed before copying source code
3. **Resource Limits**: Production compose file includes resource constraints

## Next Steps

1. **CI/CD Pipeline**: Set up automated builds and deployments
2. **Monitoring**: Add application monitoring and logging
3. **Scaling**: Configure load balancing and horizontal scaling
4. **Backup**: Set up database backup strategies 