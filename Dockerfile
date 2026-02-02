# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY RedYellowGreenBackend/RedYellowGreenBackend.csproj ./RedYellowGreenBackend/
RUN dotnet restore ./RedYellowGreenBackend/RedYellowGreenBackend.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish ./RedYellowGreenBackend/RedYellowGreenBackend.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose port 5000
EXPOSE 5001

# Set environment variables for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:5001

ENTRYPOINT ["dotnet", "RedYellowGreenBackend.dll"]

