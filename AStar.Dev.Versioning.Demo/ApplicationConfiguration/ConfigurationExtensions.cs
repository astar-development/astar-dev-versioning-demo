using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AStar.Dev.Versioning.Demo.ApplicationConfiguration;

public static class ConfigurationExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddApiVersioning(options =>
                                          {
                                              options.ReportApiVersions                   = true;
                                              options.AssumeDefaultVersionWhenUnspecified = true;
                                              options.DefaultApiVersion                   = new (2, 0);
                                              options.ApiVersionReader                    = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"));
                                          })
               .AddApiExplorer(options =>
                               {
                                   options.GroupNameFormat           = "'v'VVV";
                                   options.SubstituteApiVersionInUrl = true;
                               });

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
        builder.Services.AddSingleton<AppConfiguration>();

        builder.Services.AddOpenApi();

        return builder;
    }
}