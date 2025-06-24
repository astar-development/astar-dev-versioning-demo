using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AStar.Dev.Versioning.Demo.ApplicationConfiguration;

public sealed class ConfigureSwaggerOptions (IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        AddSwaggerDocumentForEachApiVersion(options);

        options.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
    }

    private void AddSwaggerDocumentForEachApiVersion(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
                   {
                       Title   = "Versioning API Demo",
                       Version = description.ApiVersion.ToString(),
                       Contact = new() { Name = "Someone Anyone", Email = "someemail@nowhere.com", Url = new ("https://astardevelopment.co.uk") }
                   };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated. Please review the 'api-supported-versions' header for supported versions";
        }

        return info;
    }
}