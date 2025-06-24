using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;

namespace AStar.Dev.Versioning.Demo;

public static class ConfigurationExtensions
{
    public static void Configure(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseSwagger(o => o.SerializeAsV2 = true);

        app.UseSwaggerUI(options =>
                         {
                             options.InjectStylesheet("/swagger-ui/SwaggerDark.css");

                             // build a swagger endpoint for each discovered API version
                             foreach (var description in provider.ApiVersionDescriptions)
                                 options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                                         description.GroupName.ToUpperInvariant());

                             options.DisplayOperationId();
                         });

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
