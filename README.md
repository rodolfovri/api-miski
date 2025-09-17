# ??? Arquitectura Clean Architecture - Miski API

## ?? Estructura del Proyecto

La arquitectura implementada sigue los principios de **Clean Architecture** y **Domain-Driven Design (DDD)**:

```
Miski.Api/
??? src/
?   ??? Miski.Api/                    # ?? API Layer (Controllers, Middleware)
?   ??? Miski.Application/            # ?? Business Logic (CQRS, Validators)
?   ??? Miski.Domain/                 # ?? Core Business (Entities, Contracts)
?   ??? Miski.Infrastructure/         # ??? Data Access (EF Core, Repositories)
?   ??? Miski.Shared/                 # ?? Cross-cutting (Exceptions, DTOs)
??? tests/ (pendiente)
    ??? Miski.UnitTests/
    ??? Miski.IntegrationTests/
    ??? Miski.ArchitectureTests/
```

## ?? Capas y Responsabilidades

### 1. **Domain Layer** (Corazón del Sistema)
- ? **Entidades**: `Persona`, `Producto`, `Negociacion`, `DetalleNegociacion`, `MovimientoStock`
- ? **Value Objects**: Enums (`TipoNegociacion`, `EstadoNegociacion`, etc.)
- ? **Interfaces**: `IRepository<T>`, `IUnitOfWork`, repositorios específicos
- ? **Reglas de Negocio**: Métodos como `CalcularTotal()`, `AprobarLlegadaPlanta()`

### 2. **Application Layer** (Casos de Uso)
- ? **CQRS Pattern**: Commands y Queries separadas
- ? **MediatR**: Para desacoplar handlers
- ? **FluentValidation**: Validaciones robustas
- ? **AutoMapper**: Mapeo automático de entidades a DTOs

**Implementado:**
- `CreateNegociacionCommand` + Handler + Validator
- `GetNegociacionesQuery` + Handler

### 3. **Infrastructure Layer** (Implementaciones)
- ? **Entity Framework Core**: ORM para SQL Server
- ? **Repository Pattern**: Implementación genérica + específicas
- ? **Unit of Work**: Manejo de transacciones
- ? **DbContext**: Configuración completa de entidades

### 4. **API Layer** (Presentación)
- ? **Controllers**: `NegociacionesController` con endpoints RESTful
- ? **Global Exception Handler**: Manejo centralizado de errores
- ? **Swagger**: Documentación automática de API
- ? **CORS**: Configurado para desarrollo

### 5. **Shared Layer** (Transversal)
- ? **Custom Exceptions**: `NotFoundException`, `ValidationException`, `DomainException`
- ? **DTOs**: Para transferencia de datos
- ? **Base Classes**: `BaseEntity`, `BaseDto`

## ?? Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **.NET** | 8.0 | Framework principal |
| **Entity Framework Core** | 9.0.9 | ORM y migraciones |
| **SQL Server** | LocalDB | Base de datos |
| **MediatR** | 11.0.0 | CQRS Pattern |
| **AutoMapper** | 12.0.1 | Object mapping |
| **FluentValidation** | 12.0.0 | Validaciones |
| **Swagger** | 9.0.4 | Documentación API |

## ?? Funcionalidades Implementadas

### ? **Gestión de Negociaciones**
- Crear nuevas negociaciones
- Listar negociaciones con filtros
- Validaciones robustas de entrada
- Cálculo automático de totales

### ? **Arquitectura Robusta**
- Separación clara de responsabilidades
- Inversión de dependencias
- Principios SOLID aplicados
- Patrones de diseño implementados

## ??? Configuración y Ejecución

### 1. **Base de Datos**
```bash
# Crear migración inicial
dotnet ef migrations add InitialCreate --project Miski.Infrastructure --startup-project Miski.Api

# Actualizar base de datos
dotnet ef database update --project Miski.Infrastructure --startup-project Miski.Api
```

### 2. **Ejecutar API**
```bash
dotnet run --project Miski.Api
```

### 3. **Swagger UI**
```
https://localhost:5073/swagger
```

## ?? Endpoints Disponibles

### **Negociaciones**
- `GET /api/negociaciones` - Listar negociaciones
- `GET /api/negociaciones?personaId=1` - Filtrar por persona
- `GET /api/negociaciones?estado=Pendiente` - Filtrar por estado
- `POST /api/negociaciones` - Crear negociación

**Ejemplo Request:**
```json
{
  "personaId": 1,
  "tipo": "Compra",
  "observaciones": "Negociación de prueba",
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 10,
      "precioUnitario": 15.50
    }
  ]
}
```

## ?? Próximas Mejoras

### **Funcionalidades Pendientes**
- [ ] Autenticación JWT
- [ ] Autorización basada en roles
- [ ] Gestión de stock automática
- [ ] Notificaciones por email
- [ ] Auditoría completa
- [ ] Reportes y dashboards

### **Endpoints Adicionales**
- [ ] `GET /api/negociaciones/{id}` - Obtener por ID
- [ ] `PUT /api/negociaciones/{id}/aprobar-llegada` - Aprobar llegada
- [ ] `GET /api/negociaciones/pendientes-llegada` - Pendientes de llegada
- [ ] CRUD completo para Productos y Personas

### **Mejoras Técnicas**
- [ ] Tests unitarios e integración
- [ ] Logging estructurado (Serilog)
- [ ] Health checks
- [ ] Rate limiting
- [ ] Caching (Redis)
- [ ] API Versioning

## ?? Beneficios de esta Arquitectura

### **? Escalabilidad**
- Fácil agregar nuevas funcionalidades
- Microservicios preparado
- Horizontal scaling friendly

### **? Mantenibilidad**
- Código organizado y limpio
- Responsabilidades claras
- Fácil debugging y testing

### **? Testabilidad**
- Dependencias inyectadas
- Interfaces bien definidas
- Mocking simplificado

### **? Performance**
- Queries optimizadas con EF Core
- Async/await en toda la aplicación
- Lazy loading disponible

## ?? Patrones Implementados

- **?? Clean Architecture**: Dependencias hacia adentro
- **?? CQRS**: Commands y Queries separadas
- **?? Repository Pattern**: Abstracción de datos
- **?? Unit of Work**: Transacciones coordinadas
- **??? Mapping Pattern**: AutoMapper para DTOs
- **? Mediator Pattern**: MediatR para desacoplamiento
- **??? Validation Pattern**: FluentValidation

## ?? Conclusión

Esta implementación te proporciona una base sólida, escalable y mantenible para tu sistema de gestión de negociaciones. La arquitectura está preparada para crecer y adaptarse a futuros requerimientos empresariales.

**¡Listo para implementar las funcionalidades de tu negocio! ??**"# api-miski" 
