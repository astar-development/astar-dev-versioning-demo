using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using AStar.Dev.Versioning.Demo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
                                  {
                                      // Add the headers "api-supported-versions" and "api-deprecated-versions" This is
                                      // better for discoverability
                                      options.ReportApiVersions = true;

                                      // AssumeDefaultVersionWhenUnspecified should only be enabled when supporting legacy
                                      // services that did not previously support API versioning. Forcing existing clients
                                      // to specify an explicit API version for an existing service introduces a breaking
                                      // change. Conceptually, clients in this situation are bound to some API version of
                                      // a service, but they don't know what it is and never explicit request it.
                                      options.AssumeDefaultVersionWhenUnspecified = true;
                                      options.DefaultApiVersion                   = new ApiVersion(2, 0);

                                      // Defines how an API version is read from the current HTTP request
                                      options.ApiVersionReader = ApiVersionReader.Combine(

                                                                                          // new QueryStringApiVersionReader("api-version"),
                                                                                          new HeaderApiVersionReader("api-version"));
                                  })
       .AddApiExplorer(options =>
                       {
                           // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                           // note: the specified format code will format the version as "'v'major[.minor][-status]"
                           options.GroupNameFormat = "'v'VVV";

                           // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                           // can also be used to control the format of the API version in route templates
                           options.SubstituteApiVersionInUrl = true;
                       });

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
var avdp = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.Configure(avdp);
app.MapControllers();

app.Run();
