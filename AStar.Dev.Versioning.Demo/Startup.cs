using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AStar.Dev.Versioning.Demo;

public sealed class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning(options =>
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
                                      options.DefaultApiVersion                   = new (2, 0);

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

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void Configure(IApplicationBuilder            app, IWebHostEnvironment env,
                                 IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

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
