# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the entire context
COPY . .

# Debug: List directory contents to verify files
RUN ls -la
RUN ls -la Vkr.Api || echo "Vkr.Api directory not found"

# Restore dependencies
RUN dotnet restore Vkr.Api/Vkr.API.csproj

# Build the project
RUN dotnet build Vkr.Api/Vkr.API.csproj -c Release -o /app/build

# Publish the project
RUN dotnet publish Vkr.Api/Vkr.API.csproj -c Release -o /app/publish

# Debug: List published files
RUN ls -la /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
# Debug: List files in runtime stage
RUN ls -la
ENTRYPOINT ["dotnet", "Vkr.API.dll"]