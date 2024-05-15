using AutoGenerateAPI.Database.Models;
using log4net;
using log4net.Config;
using System.Reflection;
using Microsoft.SqlServer;
using Microsoft.EntityFrameworkCore;
using AutoGenerateAPI.Interface;
using AutoGenerateAPI.Repository;

void ConfigureLog4net()
{
    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
}

var builder = WebApplication.CreateBuilder(args);

ConfigureLog4net();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TryAutomationContext>(options => options.UseSqlServer(connectionString, serverOptions =>
{
    serverOptions.EnableRetryOnFailure(
        maxRetryCount: 3,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null
        );
    ;
}));


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddTransient<IGET, GETRepository>();
builder.Services.AddTransient<IPOST, POSTRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
