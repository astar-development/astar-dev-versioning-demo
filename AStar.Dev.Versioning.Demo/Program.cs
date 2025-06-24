using Asp.Versioning.ApiExplorer;
using AStar.Dev.Versioning.Demo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.ConfigureApplication(apiVersionDescriptionProvider);

app.Run();
