using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using WCNApi.Applications.Services;
using WCNApi.core.Data;
using WCNApi.core.Data.Repositories;
using WCNApi.core.Domain.Repositories;
using WCNApi.Infrastructure.DependencyInjection;
using WCNApi.Infrastructure.Helpers;
using WCNApi.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region Serilog Configuration

var year = DateTime.UtcNow.ToString("yyyy", CultureInfo.InvariantCulture);
var month = DateTime.UtcNow.ToString("MM", CultureInfo.InvariantCulture);
var currentDate = DateTime.UtcNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

// Logs/2026/10
var monthFolder = Path.Combine("Logs", year, month);
Directory.CreateDirectory(monthFolder);

// Logs/2026/10/Exceptions
var exceptionFolder = Path.Combine(monthFolder, "Exceptions");
Directory.CreateDirectory(exceptionFolder);

// Main log file
var logFilePath = Path.Combine(monthFolder, $"{currentDate}_log.txt");

// Exception-only log file
var exceptionLogFilePath = Path.Combine(exceptionFolder, $"{currentDate}_exception_log.txt");

const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] " + "[{SourceContext}] " + "{Message:lj}{NewLine}{Exception}";
Log.Logger = new LoggerConfiguration().MinimumLevel.Information().Enrich.FromLogContext().WriteTo.Console(outputTemplate: outputTemplate)
    // All application logs
    .WriteTo.File(path: logFilePath, outputTemplate: outputTemplate, rollingInterval: RollingInterval.Infinite, shared: true)
    // Only logs containing Exception object
    .WriteTo.Logger(exceptionLogger => exceptionLogger
            .Filter.ByIncludingOnly(logEvent => logEvent.Exception != null)
            .WriteTo.File(path: exceptionLogFilePath, outputTemplate: outputTemplate, rollingInterval: RollingInterval.Infinite, shared: true)).CreateLogger();

// ILogger<T> will now use Serilog
builder.Host.UseSerilog();

#endregion

// Load Crypto settings from appsettings.json
CryptoHelper.SecretKey = builder.Configuration["Crypto:SecretKey"] ?? 
    throw new InvalidOperationException("Crypto:SecretKey is not configured.");

CryptoHelper.ResponseSecretKey = builder.Configuration["Crypto:ResponseSecretKey"] ?? 
    throw new InvalidOperationException("Crypto:ResponseSecretKey is not configured.");

// Add services to the container.
//SetDependencyInjectionDefinitions(builder.Services, builder.Configuration);

// DEPENDENCY INJECTION
builder.Services.AddApplicationDependencies(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

// HTTP Pipeline
app.UseHttpsRedirection();

// Required because EncryptionMiddleware
// reads controller name using GetRouteData()
app.UseRouting();

// Custom Middleware
// Captures the API response and encrypts HTTP 200 responses
//app.UseMiddleware<EncryptionMiddleware>();

// Handles unexpected exceptions from
// Controller / Service / Repository / Database
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

// Controller Endpoints
app.MapControllers();

app.Run();



//static void SetDependencyInjectionDefinitions(IServiceCollection services, IConfiguration configuration)
//{
//    // register db service 
//    services.AddDbContext<AppDbContext>(
//        options =>
//        {
//            options.UseSqlServer(
//                configuration.GetConnectionString(
//                    "DefaultConnection"));
//        });

//    services.AddScoped<IDbContext>(
//        provider =>
//            provider.GetRequiredService<AppDbContext>());

//    // register services
//    services.AddScoped<IUserService, UserService>();

//    // register repos
//    services.AddScoped<IUserRepository, UserRepository>();
//}