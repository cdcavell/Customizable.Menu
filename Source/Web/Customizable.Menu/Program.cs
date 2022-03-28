using Customizable.Menu;
using Serilog;

var config = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets(typeof(Program).Assembly, optional: true)
    .AddCommandLine(args)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateBootstrapLogger();

Log.Information("Starting Host");

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Adding services to the container");
    var app = builder.ConfigureServices();

    Log.Information("Configuring the HTTP request pipeline");
    app.ConfigurePipeline();

    Log.Information("Starting application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
