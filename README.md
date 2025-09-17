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

### 1. **Domain Layer** (Coraz�n del Sistema)
- ? **Entidades**: `Persona`, `Producto`, `Negociacion`, `DetalleNegociacion`, `MovimientoStock`
- ? **Value Objects**: Enums (`TipoNegociacion`, `EstadoNegociacion`, etc.)
- ? **Interfaces**: `IRepository<T>`, `IUnitOfWork`, repositorios espec�ficos
- ? **Reglas de Negocio**: M�todos como `CalcularTotal()`, `AprobarLlegadaPlanta()`

### 2. **Application Layer** (Casos de Uso)
- ? **CQRS Pattern**: Commands y Queries separadas
- ? **MediatR**: Para desacoplar handlers
- ? **FluentValidation**: Validaciones robustas
- ? **AutoMapper**: Mapeo autom�tico de entidades a DTOs

**Implementado:**
- `CreateNegociacionCommand` + Handler + Validator
- `GetNegociacionesQuery` + Handler

### 3. **Infrastructure Layer** (Implementaciones)
- ? **Entity Framework Core**: ORM para SQL Server
- ? **Repository Pattern**: Implementaci�n gen�rica + espec�ficas
- ? **Unit of Work**: Manejo de transacciones
- ? **DbContext**: Configuraci�n completa de entidades

### 4. **API Layer** (Presentaci�n)
- ? **Controllers**: `NegociacionesController` con endpoints RESTful
- ? **Global Exception Handler**: Manejo centralizado de errores
- ? **Swagger**: Documentaci�n autom�tica de API
- ? **CORS**: Configurado para desarrollo

### 5. **Shared Layer** (Transversal)
- ? **Custom Exceptions**: `NotFoundException`, `ValidationException`, `DomainException`
- ? **DTOs**: Para transferencia de datos
- ? **Base Classes**: `BaseEntity`, `BaseDto`

## ?? Tecnolog�as Utilizadas

| Tecnolog�a | Versi�n | Prop�sito |
|------------|---------|-----------|
| **.NET** | 8.0 | Framework principal |
| **Entity Framework Core** | 9.0.9 | ORM y migraciones |
| **SQL Server** | LocalDB | Base de datos |
| **MediatR** | 11.0.0 | CQRS Pattern |
| **AutoMapper** | 12.0.1 | Object mapping |
| **FluentValidation** | 12.0.0 | Validaciones |
| **Swagger** | 9.0.4 | Documentaci�n API |

## ?? Funcionalidades Implementadas

### ? **Gesti�n de Negociaciones**
- Crear nuevas negociaciones
- Listar negociaciones con filtros
- Validaciones robustas de entrada
- C�lculo autom�tico de totales

### ? **Arquitectura Robusta**
- Separaci�n clara de responsabilidades
- Inversi�n de dependencias
- Principios SOLID aplicados
- Patrones de dise�o implementados

## ??? Configuraci�n y Ejecuci�n

### 1. **Base de Datos**
```bash
# Crear migraci�n inicial
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
- `POST /api/negociaciones` - Crear negociaci�n

**Ejemplo Request:**
```json
{
  "personaId": 1,
  "tipo": "Compra",
  "observaciones": "Negociaci�n de prueba",
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 10,
      "precioUnitario": 15.50
    }
  ]
}
```

## ?? Pr�ximas Mejoras

### **Funcionalidades Pendientes**
- [ ] Autenticaci�n JWT
- [ ] Autorizaci�n basada en roles
- [ ] Gesti�n de stock autom�tica
- [ ] Notificaciones por email
- [ ] Auditor�a completa
- [ ] Reportes y dashboards

### **Endpoints Adicionales**
- [ ] `GET /api/negociaciones/{id}` - Obtener por ID
- [ ] `PUT /api/negociaciones/{id}/aprobar-llegada` - Aprobar llegada
- [ ] `GET /api/negociaciones/pendientes-llegada` - Pendientes de llegada
- [ ] CRUD completo para Productos y Personas

### **Mejoras T�cnicas**
- [ ] Tests unitarios e integraci�n
- [ ] Logging estructurado (Serilog)
- [ ] Health checks
- [ ] Rate limiting
- [ ] Caching (Redis)
- [ ] API Versioning

## ?? Beneficios de esta Arquitectura

### **? Escalabilidad**
- F�cil agregar nuevas funcionalidades
- Microservicios preparado
- Horizontal scaling friendly

### **? Mantenibilidad**
- C�digo organizado y limpio
- Responsabilidades claras
- F�cil debugging y testing

### **? Testabilidad**
- Dependencias inyectadas
- Interfaces bien definidas
- Mocking simplificado

### **? Performance**
- Queries optimizadas con EF Core
- Async/await en toda la aplicaci�n
- Lazy loading disponible

## ?? Patrones Implementados

- **?? Clean Architecture**: Dependencias hacia adentro
- **?? CQRS**: Commands y Queries separadas
- **?? Repository Pattern**: Abstracci�n de datos
- **?? Unit of Work**: Transacciones coordinadas
- **??? Mapping Pattern**: AutoMapper para DTOs
- **? Mediator Pattern**: MediatR para desacoplamiento
- **??? Validation Pattern**: FluentValidation

## ?? Conclusi�n

Esta implementaci�n te proporciona una base s�lida, escalable y mantenible para tu sistema de gesti�n de negociaciones. La arquitectura est� preparada para crecer y adaptarse a futuros requerimientos empresariales.

**�Listo para implementar las funcionalidades de tu negocio! ??**"# api-miski" 
