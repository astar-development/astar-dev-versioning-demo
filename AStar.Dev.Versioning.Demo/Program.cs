using AStar.Dev.Versioning.Demo.ApplicationConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

var appConfiguration = app.Services.GetRequiredService<AppConfiguration>();

appConfiguration.ConfigureApplication(app);

app.Run();