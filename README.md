# RedYellowGreenBackend

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQLite](https://www.sqlite.org/download.html) (optional, only if you want to inspect the database file)

## Running the Application

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

4. **API Endpoints:**
   - Equipment: `http://localhost:5000/api/equipment`
   - SignalR Hub: `http://localhost:5000/hubs/states`

## CORS Configuration
- By default, the backend allows requests from any frontend (any origin).
- If you want to restrict CORS, update the CORS policy in `Program.cs`.

## Database
- The backend uses a SQLite database file at `./TestDatabase/RedYellowGreenBackend.db`.
- The database is automatically created and seeded with test data on startup.

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
- To run integration and unit tests:
  ```sh
  dotnet test
  ```

## Project Structure
- `RedYellowGreenBackend/` - Main backend project
- `RedYellowGreenBackend.Tests/` - Test project
- `TestDatabase/RedYellowGreenBackend.db` - SQLite database file

