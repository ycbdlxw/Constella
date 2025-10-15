using myapp;
using myapp.Extensions;
using myapp.Services;
using Serilog;
using FluentMigrator.Runner;
using System.Reflection;
using System.IO;
using System.Linq;

// Setup serilog
LogHelper.SetupSerilog();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services;
    var configuration = builder.Configuration;

    // Add services to the container.
    builder.Host.UseSerilog();
    services.AddHttpContextAccessor();
    services.AddScoped<IUserContext, UserContext>();
    
    // Database services
    services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
    services.AddSingleton<DatabaseService>();
    services.AddScoped<CommonService>(); // Register CommonService

    // MVC Services
    services.AddControllers();

    // Plugin Engine
    var pluginEngine = new PluginEngine();
    pluginEngine.LoadPlugins(); // Default path is "src/Plugins"
    services.AddSingleton(pluginEngine);

    // Get all assemblies to scan for migrations (main app + plugins)
    var assembliesToScan = new[] { Assembly.GetExecutingAssembly() }
        .Concat(pluginEngine.LoadedAssemblies)
        .ToArray();
    Log.Information("Scanning {Count} assemblies for migrations.", assembliesToScan.Length);

    // Add FluentMigrator with database provider switching
    services.AddFluentMigratorCore()
        .ConfigureRunner(rb => 
        {
            var dbType = configuration.GetValue<string>("Database:Type");
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            switch (dbType?.ToLower())
            {
                case "sqlite":
                    rb.AddSQLite();
                    break;
                default:
                    throw new NotSupportedException($"Database type '{dbType}' is not supported.");
            }

            rb.WithGlobalConnectionString(connectionString)
              .ScanIn(assembliesToScan).For.Migrations();
        })
        .AddLogging(lb => lb.AddFluentMigratorConsole());


    var port = Environment.GetEnvironmentVariable("PORT") ?? "8081";
    var url = $"http://0.0.0.0:{port}";

    var app = builder.Build();

    // Automatically run database migrations on startup
    using (var scope = app.Services.CreateScope())
    {
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        Log.Information("Running database migrations...");
        
        var dbType = configuration.GetValue<string>("Database:Type");
        if (dbType?.ToLower() == "sqlite")
        {
             var connectionString = configuration.GetConnectionString("DefaultConnection");
             var dataSource = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connectionString).DataSource;
             if (dataSource != null) {
                var dbDirectory = Path.GetDirectoryName(dataSource);
                if (!string.IsNullOrEmpty(dbDirectory) && !Directory.Exists(dbDirectory))
                {
                    Log.Information($"Creating database directory: {dbDirectory}");
                    Directory.CreateDirectory(dbDirectory);
                }
             }
        }

        runner.MigrateUp();
        Log.Information("Database migrations completed.");
    }

    // Configure the HTTP request pipeline.
    app.UseGlobalExceptionMiddleware();
    app.UseRouting();
    app.MapControllers(); // Map controller routes

    app.Run(url);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
