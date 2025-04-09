# Use the .NET 8 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution and restore dependencies
COPY ../*.sln ./
COPY ../Benchmarking/Benchmarking.csproj ./Benchmarking/
COPY ../EntityBenchmark/EntityBenchmark.csproj ./EntityBenchmark/
COPY ../DapperBenchmark/DapperBenchmark.csproj ./DapperBenchmark/
RUN dotnet restore

# Copy the entire source code and build the application
COPY ../ ./
RUN dotnet build -c Release

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "Benchmarking.dll"]
