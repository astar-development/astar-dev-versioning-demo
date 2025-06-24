using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;

namespace AStar.Dev.Versioning.Demo.ApplicationConfiguration;

public class AppConfiguration(IApiVersionDescriptionProvider provider)
{
    public void ConfigureApplication(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSwagger(o => o.SerializeAsV2 = true);
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
                         {
                             options.InjectStylesheet("/swagger-ui/SwaggerDark.css");

                             foreach (var description in provider.ApiVersionDescriptions)
                             {
                                 options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                             }

                             options.DisplayOperationId();
                         });

        app.UseAuthorization();
        app.MapControllers();
    }
}