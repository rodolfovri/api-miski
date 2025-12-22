using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Miski.Api.Controllers.Filters;
using Miski.Application.Features.Auth.Commands.Login;
using Miski.Application.Features.Compras.Negociaciones.Commands.CreateNegociacion;
using Miski.Application.Mappings;
using Miski.Domain.Contracts;
using Miski.Domain.Contracts.Repositories;
using Miski.Infrastructure.Data;
using Miski.Infrastructure.Persistence;
using Miski.Infrastructure.Repositories;
using Miski.Shared.DTOs.Base;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ CONFIGURAR LÍMITES DE KESTREL PARA ARCHIVOS GRANDES
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 100 * 1024 * 1024; // 100 MB
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
});

// ✅ CONFIGURAR LÍMITES DE FORMDATA
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100 MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
    options.BufferBodyLengthLimit = 100 * 1024 * 1024; // 100 MB
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Miski API", 
        Version = "v1",
        Description = "API para el sistema de gestión agrícola Miski",
        Contact = new OpenApiContact
        {
            Name = "Equipo Miski",
            Email = "soporte@miski.com"
        }
    });
    
    c.EnableAnnotations();
    
    // Configuración para agrupar por Tags (módulos)
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((name, api) => true);

    // JWT Configuration for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. \r\n\r\n " +
                      "Ingresa 'Bearer' [espacio] y luego tu token en el campo de texto abajo.\r\n\r\n" +
                      "Ejemplo: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            },
    //            Scheme = "oauth2",
    //            Name = "Bearer",
    //            In = ParameterLocation.Header,
    //        },
    //        new List<string>()
    //    }
    //});

    //// Incluir comentarios XML si existen
    //var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //if (File.Exists(xmlPath))
    //{
    //    c.IncludeXmlComments(xmlPath);
    //}

    // Filtro personalizado para autorización
    c.OperationFilter<AuthorizeOperationFilter>();
});

// Database Configuration - PostgreSQL
builder.Services.AddDbContext<MiskiDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        // Configurar encoding UTF-8
        npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
    
    // Configurar Npgsql para usar UTF-8
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
});

// JWT Configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? "MiskiSecretKey2024!@#$%";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MiskiApi";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MiskiClient";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// MediatR Configuration
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateNegociacionHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly);
});

// AutoMapper Configuration con conversión automática de fechas UTC a local
builder.Services.AddAutoMapper((Action<IServiceProvider, IMapperConfigurationExpression>)((serviceProvider, cfg) =>
{
    // Obtener el servicio de fecha/hora del contenedor de DI
    var dateTimeService = serviceProvider.GetRequiredService<Miski.Application.Services.IDateTimeService>();
    
    // Aplicar conversión automática a TODAS las propiedades DateTime y DateTime?
    cfg.CreateMap<DateTime, DateTime>().ConvertUsing((src, dest, context) => 
        dateTimeService.ConvertToLocalTime(src));
    
    cfg.CreateMap<DateTime?, DateTime?>().ConvertUsing((src, dest, context) => 
        dateTimeService.ConvertToLocalTime(src));
    
    // Agregar el perfil de mapeos
    cfg.AddProfile<Miski.Application.Mappings.MappingProfile>();
}), typeof(MappingProfile).Assembly);

// FluentValidation Configuration
builder.Services.AddValidatorsFromAssemblyContaining<CreateNegociacionValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Repository Pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INegociacionRepository, NegociacionRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

// File Storage Service
builder.Services.AddScoped<Miski.Application.Services.IFileStorageService, Miski.Application.Services.LocalFileStorageService>();
// TODO: Para producción, cambiar a:
// builder.Services.AddScoped<Miski.Application.Services.IFileStorageService, Miski.Application.Services.CloudFileStorageService>();

// DateTime Service - Conversión de zona horaria
builder.Services.AddSingleton<Miski.Application.Services.IDateTimeService, Miski.Application.Services.DateTimeService>();

// Configuracion Service - Obtener configuraciones globales
builder.Services.AddScoped<Miski.Application.Services.IConfiguracionService, Miski.Application.Services.ConfiguracionService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Habilitar Swagger en todos los entornos (Development y Production)
// NOTA: En producción real, considera proteger Swagger con autenticación
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Miski API v1");
    c.RoutePrefix = "swagger";
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    c.DefaultModelsExpandDepth(-1);
    c.DisplayRequestDuration();
    
    // Personalización de la interfaz
    c.DocumentTitle = "Miski API - Documentación";
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Servir archivos estáticos para Swagger UI
app.UseStaticFiles();

// Configurar servicio de archivos estáticos para las imágenes de negociaciones
var uploadPath = builder.Configuration["FileStorage:BasePath"] ?? 
                 Path.Combine(builder.Environment.ContentRootPath, "uploads");

// Crear el directorio si no existe
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadPath),
    RequestPath = "/uploads",
    OnPrepareResponse = ctx =>
    {
        // Configurar CORS headers para las imágenes
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
        
        // Cache control para las imágenes (opcional)
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
    }
});

// Global Exception Handler
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MiskiDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Aplicando migraciones pendientes...");
        context.Database.Migrate();
        logger.LogInformation("Migraciones aplicadas exitosamente");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al aplicar migraciones");
        throw;
    }
}

app.Run();

// Global Exception Middleware
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            Miski.Shared.Exceptions.NotFoundException => ApiResponse.ErrorResult(
                exception.Message, 
                "Recurso no encontrado"
            ),
            Miski.Shared.Exceptions.ValidationException validationEx => ApiResponse.ValidationErrorResult(
                validationEx.Errors
            ),
            Miski.Shared.Exceptions.DomainException => ApiResponse.ErrorResult(
                exception.Message, 
                "Error de regla de negocio"
            ),
            FluentValidation.ValidationException fluentEx => ApiResponse.ValidationErrorResult(
                fluentEx.Errors.Select(e => new { 
                    Field = e.PropertyName, 
                    Error = e.ErrorMessage 
                })
            ),
            _ => ApiResponse.ErrorResult(
                "Ocurrió un error interno en el servidor", 
                exception.Message
            )
        };

        context.Response.StatusCode = exception switch
        {
            Miski.Shared.Exceptions.NotFoundException => 404,
            Miski.Shared.Exceptions.ValidationException => 400,
            Miski.Shared.Exceptions.DomainException => 400,
            FluentValidation.ValidationException => 400,
            _ => 500
        };

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}