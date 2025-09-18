using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Miski.Api.Controllers.NewFolder;
using Miski.Application.Features.Auth.Commands.Login;
using Miski.Application.Features.Negociaciones.Commands.CreateNegociacion;
using Miski.Application.Mappings;
using Miski.Domain.Contracts;
using Miski.Domain.Contracts.Repositories;
using Miski.Infrastructure.Data;
using Miski.Infrastructure.Persistence;
using Miski.Infrastructure.Repositories;
using Miski.Shared.DTOs.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Miski API", Version = "v1" });
    c.EnableAnnotations();
    
    // JWT Configuration for Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Apllicar el filtro de operación personalizado
    c.OperationFilter<AuthorizeOperationFilter>();

    //c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    //{
    //    {
    //        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    //        {
    //            Reference = new Microsoft.OpenApi.Models.OpenApiReference
    //            {
    //                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            },
    //            Scheme = "oauth2",
    //            Name = "Bearer",
    //            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    //        },
    //        new List<string>()
    //    }
    //});
});

// Database Configuration
builder.Services.AddDbContext<MiskiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// AutoMapper Configuration
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// FluentValidation Configuration
builder.Services.AddValidatorsFromAssemblyContaining<CreateNegociacionValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Repository Pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INegociacionRepository, NegociacionRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Global Exception Handler
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication(); // Agregar antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();

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
