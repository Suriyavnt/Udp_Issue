# Use a base image for the .NET SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the csproj and restore the dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire application code
COPY . ./

# Publish the application in release mode as framework-dependent
RUN dotnet publish -c Release -r linux-x64 --self-contained false -o out

# Use a runtime image for the application (ASP.NET Core runtime image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory for the app
WORKDIR /app

# Copy the app from the build stage
COPY --from=build /app/out ./

# Set the entry point for the app
ENTRYPOINT ["dotnet", "CoreClientApp.dll"]
