using System;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AStar.Dev.Versioning.Demo;

public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));

        options.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Versioning API Demo",
            Version = description.ApiVersion.ToString(),
            Contact = new OpenApiContact
            {
                Name = "Someone Anyone", Email = "someemail@nowhere.com", Url = new Uri("https://www.capgemini.com")
            }
        };

        if (description.IsDeprecated) info.Description += " This API version has been deprecated.";

        return info;
    }
}