using MediatR;
using Microsoft.EntityFrameworkCore;
using Miski.Application.Mappings;
using Miski.Domain.Contracts;
using Miski.Domain.Contracts.Repositories;
using Miski.Infrastructure.Data;
using Miski.Infrastructure.Persistence;
using Miski.Infrastructure.Repositories;
using Miski.Application.Features.Negociaciones.Commands.CreateNegociacion;
using FluentValidation;
using Swashbuckle.AspNetCore.Annotations;
using Miski.Shared.DTOs.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Miski API", Version = "v1" });
    c.EnableAnnotations();
});

// Database Configuration
builder.Services.AddDbContext<MiskiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR Configuration
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateNegociacionHandler).Assembly);
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// FluentValidation Configuration
builder.Services.AddValidatorsFromAssemblyContaining<CreateNegociacionValidator>();

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
