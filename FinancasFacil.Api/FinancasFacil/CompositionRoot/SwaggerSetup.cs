using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FinancasFacil.Api.CompositionRoot;

public static class SwaggerSetup
{
    public static IServiceCollection AddSwaggerWithOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Calendário API",
                Version = "v1",
                Description = "API para consulta de feriados (com Dapper + Clean Architecture)."
            });

            // inclui comentários XML (habilite <GenerateDocumentationFile>true</GenerateDocumentationFile> no .csproj da API)
            var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        });

        return services;
    }
}
