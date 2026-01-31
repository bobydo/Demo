# EPS Demo Architecture

## Structure

```
Demo/
│
├── src/
│   ├── Demo.Web/                # Blazor WASM UI (grid, drill-down)
│   ├── Demo.Application/        # Application services (TripProcessor, DTOs)
│   ├── Demo.Domain/             # Domain entities, interfaces, logic
│   ├── Demo.Infrastructure/     # EF Core, data access, CSV seeding
│   └── Demo.Tests/              # Unit tests
│
├── Data/                           # CSV files (seed data)
└── README.md
```
## Database Deployment
To set up and deploy the database schema using EF Core migrations:
```sh
dotnet ef migrations add InitialCreate --project src/Demo.Infrastructure --startup-project src/Demo.Server
dotnet ef database update --project src/Demo.Infrastructure --startup-project src/Demo.Server
```

**Default: SQLite (Recommended for reviewers)**
- Extension to view db file https://marketplace.visualstudio.com/items?itemName=alexcvzz.vscode-sqlite
![alt text](image.png)
- The server will use SQLite by default for easy setup. The database file will be created at `Database/Demo.db`.
- taskkill /IM dotnet.exe /F

**To use SQL Server Express (as neded):**
- Set the environment variable `UseSqlite=false` or add `"UseSqlite": false` to your `appsettings.Development.json`.
- Optionally, set your own connection string in `ConnectionStrings:SqlExpress`.
- Example connection string: `Server=.\\SQLEXPRESS;Database=Demo;Trusted_Connection=True;MultipleActiveResultSets=true`

---
## Why Use Demo.Server for EF Core Migrations

- **Demo.Server.csproj** is required as the startup project for EF Core migrations because it contains the application's entry point, configuration, and dependency injection setup. It is used for running and applying migrations, not for direct database CRUD logic.
- **Demo.Infrastructure.csproj** contains the DbContext and entity definitions, and provides all database CRUD services for the application. Migrations are added here, but it cannot run or provide configuration by itself.
- **Demo.Web.csproj** is a Blazor WebAssembly project and cannot be used for migrations because it runs in the browser and does not support server-side EF Core operations.


## Event-to-Trip Grouping Logic

The following logic is used to group and assign events to trips based on event codes:
```
For each EquipmentId:
  currentTrip = null

  For each event in time order:
	if EventCode == "W":
		start new trip
		currentTrip = trip

	else if EventCode == "Z" AND currentTrip != null:
		end currentTrip
		currentTrip = null

	else:
		if currentTrip != null:
			attach event (A, D, etc.) to currentTrip
		else:
			ignore or store as orphan event
```

This logic ensures that trips are started with a "W" event, ended with a "Z" event, and all other events (such as "A", "D", etc.) are only attached to an active trip. Events outside of a trip are ignored or stored as orphans.

---
## Running the Web Project & Freeing Port 5001


```powershell
dotnet workload install wasm-tools
taskkill /IM dotnet.exe /F
dotnet watch --project src/Demo.Server/Demo.Server.csproj
dotnet watch --project src/Demo.Web/Demo.Web.csproj
```

- Result
![alt text](image-1.png)


