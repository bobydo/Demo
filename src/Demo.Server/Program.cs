
using Serilog;
using Demo.Infrastructure;
using Demo.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Demo.Application.Services;
using Demo.Server.Services;




var builder = WebApplication.CreateBuilder(args);

// Add CORS policy for Blazor WASM
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
        policy => policy
            .WithOrigins("https://localhost:5001")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventCodeDefinitionService, EventCodeDefinitionService>();
builder.Services.AddScoped<ITripService, TripService>();

builder.Services.AddMemoryCache();
builder.Services.AddControllers();

// Add shared configuration from src/appsettings.shared.json
builder.Configuration.AddJsonFile(Path.Combine("..", "appsettings.shared.json"), optional: true, reloadOnChange: true);
var serilogSection = builder.Configuration.GetSection("Serilog");
Console.WriteLine("Serilog config found: " + serilogSection.Exists());

// Use SQLite by default for reviewers, fallback to SQL Server Express if specified
var sqlitePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Database", "Demo.db");
var sqliteConn = $"Data Source={sqlitePath}";
var sqlExpressConn = builder.Configuration["ConnectionStrings:SqlExpress"] ??
    "Server=.\\SQLEXPRESS;Database=EPS_Demo;Trusted_Connection=True;MultipleActiveResultSets=true";
var useSqlite = builder.Configuration.GetValue<bool>("UseSqlite", true);

if (useSqlite)
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(sqliteConn));
else
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlExpressConn));

// Register the data seeder
builder.Services.AddScoped<IDataSeeder, CsvDataSeeder>();


// Configure Serilog from configuration (shared + local)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();


// Force Development environment for Swagger
builder.Environment.EnvironmentName = "Development";
var app = builder.Build();

// Test Serilog logging
Log.Information("Serilog test: Demo.Server has started and logging is working.");

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS for Blazor WASM
app.UseCors("AllowBlazorWasm");


// Run migrations and seed data at startup (seed only if tables are empty)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
}



app.MapGet("/", () => "Demo.Server is running. DB is up-to-date.");
app.MapControllers();

// Print Swagger URL(s) on startup
var urls = app.Urls.Any() ? app.Urls : new[] { "http://localhost:5000"};
foreach (var url in urls)
{
    Console.WriteLine($"Swagger UI: {url}/swagger");
}

app.Run();
