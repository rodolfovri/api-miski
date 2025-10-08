using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Miski.Api.Controllers.NewFolder;

public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verificar si el método o la clase tienen el atributo [Authorize]
        var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                          || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        // Verificar si hay [AllowAnonymous] que override el [Authorize]
        var hasAllowAnonymous = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any()
                               || context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

        // Si tiene [Authorize] y no tiene [AllowAnonymous], aplicar seguridad
        if (hasAuthorize && !hasAllowAnonymous)
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };

            // Opcional: Agregar descripción de respuesta 401
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "No autorizado" });
        }
    }
}