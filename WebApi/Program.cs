using Domain.Dtos;
using Infrastructure.AutoMapper;
using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using WebApi.ExtensionMethods.AuthConfiguration;
using WebApi.ExtensionMethods.RegisterService;
using WebApi.ExtensionMethods.SwaggerConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig!);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// connection to database && dependency injection
builder.Services.AddRegisterService(builder.Configuration);
builder.Services.AddCors();

// register swagger configuration
builder.Services.SwaggerService();

// authentications service
builder.Services.AddAuthConfigureService(builder.Configuration);


// automapper
builder.Services.AddAutoMapper(typeof(MapperProfile));


var app = builder.Build();


app.UseCors(
    builder => builder.WithOrigins("http://127.0.0.1:5500", "http://localhost:3000","https://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
);

// update database
try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider; 
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
    
    //seed data
    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.SeedRole();
    await seeder.SeedUser();
}
catch (Exception e)
{
    // ignored
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
