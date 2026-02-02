# RedYellowGreenBackend

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (if not using Docker)
- [Git](https://git-scm.com/)
- [Docker](https://www.docker.com/get-started) (recommended for easiest setup)
- [SQLite](https://www.sqlite.org/download.html) (optional, only if you want to inspect the database file)

## Getting Started

### Option 1: Run with Docker (Recommended)

1. **Clone the repository:**
   ```sh
   git clone https://github.com/KamilleNikolajsen/RedYellowGreenBackend.git
   cd RedYellowGreenBackend
   ```
2. **Build and run the backend:**
   ```sh
   docker-compose up --build
   ```
   The backend will start on `http://localhost:5000` by default.

### Option 2: Run Locally (without Docker)

1. **Restore dependencies:**
   ```sh
   dotnet restore
   ```
2. **Build the project:**
   ```sh
   dotnet build
   ```
3. **Run the backend:**
   ```sh
   dotnet run --project RedYellowGreenBackend/RedYellowGreenBackend.csproj
   ```
   The backend will start on `http://localhost:5000` by default.

## API Endpoints
- Equipment: `http://localhost:5000/api/equipment`
- SignalR Hub: `http://localhost:5000/hubs/states`

## CORS Configuration
- By default, the backend allows requests from `http://localhost:5173` (Vite/React default). To allow other frontends, update the CORS policy in `Program.cs`.

## Database
- The backend uses a SQLite database file at `./TestDatabase/RedYellowGreenBackend.db`.
- The database is automatically created and seeded with test data on startup. No manual setup is required.
- When running with Docker, the database is persisted in the `TestDatabase` folder on your host machine.

## SignalR
- Real-time equipment state updates are available via the SignalR hub at `/hubs/states`.
- Connect from your frontend using:
  ```js
  const connection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5000/hubs/states')
    .withAutomaticReconnect()
    .build();
  ```

## Running Tests
- To run integration and unit tests (locally):
  ```sh
  dotnet test
  ```

## Troubleshooting
- If you see errors about missing packages, run `dotnet restore`.
- If you see errors about database files, ensure you have write permissions to the `TestDatabase` folder.
- If you need to change the frontend origin, update the CORS policy in `Program.cs`.
- If you get port conflicts, stop other applications using port 5000 or change the port in `Program.cs` or `docker-compose.yml`.

## Project Structure
- `RedYellowGreenBackend/` - Main backend project
- `RedYellowGreenBackend.Tests/` - Test project
- `TestDatabase/RedYellowGreenBackend.db` - SQLite database file

