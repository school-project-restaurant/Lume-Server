# syntax=docker/dockerfile:1

ARG DOTNET_VERSION=9.0
ARG DB_CONNECTION_STRING

# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS builder
WORKDIR /app

# Copy csproj files and restore dependencies
COPY src/Lume.API/Lume.API.csproj src/Lume.API/
COPY src/Lume.Application/Lume.Application.csproj src/Lume.Application/
COPY src/Lume.Domain/Lume.Domain.csproj src/Lume.Domain/
COPY src/Lume.Infrastructure/Lume.Infrastructure.csproj src/Lume.Infrastructure/

RUN dotnet restore "src/Lume.API/Lume.API.csproj"

# Copy everything else and build
COPY src/Lume.API/ src/Lume.API/
COPY src/Lume.Application/ src/Lume.Application/
COPY src/Lume.Domain/ src/Lume.Domain/
COPY src/Lume.Infrastructure/ src/Lume.Infrastructure/
COPY utility/ utility/

RUN dotnet publish "src/Lume.API/Lume.API.csproj" -c Release -o /app/publish --no-restore

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
WORKDIR /app

# Create data directory with proper permissions
RUN mkdir -p /data && \
    chown -R 1000:1000 /data && \
    chmod 700 /data # 700

RUN mkdir -p /utility
COPY --from=builder /app/utility /utility
COPY --from=builder /app/publish .

# Environment variables
ENV ASPNETCORE_URLS=http://+:5155
ENV DB_CONNECTION_STRING=$DB_CONNECTION_STRING

RUN echo "DB_CONNECTION_STRING=$DB_CONNECTION_STRING" > /app/src/Lume.API/Secrets.env

EXPOSE 5155

# Run as non-root user
USER 1000:1000

ENTRYPOINT ["dotnet", "Lume.API.dll"]