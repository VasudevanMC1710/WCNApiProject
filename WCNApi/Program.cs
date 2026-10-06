
using WCNApi.Applications.Services;
using WCNApi.core.Data;
using WCNApi.core.Data.Repositories;
using WCNApi.core.Domain.Repositories;
using WCNApi.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SetDependencyInjectionDefinitions(builder.Services, builder.Configuration);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Custom middleware should be added after authentication and authorization
app.UseMiddleware<ExceptionMiddleware>();

app.Run();



static void SetDependencyInjectionDefinitions(IServiceCollection services, IConfiguration configuration)
{
    // register db service 
    services.AddSingleton<AppDbContext>();

    // register services
    services.AddScoped<IUserService, UserService>();

    // register repos
    services.AddScoped<IUserRepository, UserRepository>();
}