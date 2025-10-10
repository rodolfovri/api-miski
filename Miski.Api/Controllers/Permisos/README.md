# Módulo de Permisos

Este módulo gestiona el sistema de permisos jerárquico con 3 niveles: Módulo ? SubMódulo ? SubMóduloDetalle.

## Funcionalidades principales:
- Gestión de permisos por rol con herencia jerárquica
- Estructura de 3 niveles: Módulo, SubMódulo y SubMóduloDetalle
- Asignación y revocación de permisos
- Consulta de permisos con jerarquía completa

## Controladores:
- `PermisosController`: Gestión completa de permisos por rol

## Lógica de Permisos

### Herencia Jerárquica:
- **Si un rol tiene acceso a un Módulo**: Automáticamente accede a todos sus SubMódulos y SubMóduloDetalles
- **Si tiene acceso solo a un SubMódulo**: Solo accede a ese SubMódulo y sus SubMóduloDetalles
- **Si tiene acceso solo a un SubMóduloDetalle**: Acceso limitado únicamente a ese detalle

### Ejemplo:
```
Módulo: Compras (TieneAcceso = true)
  ??? SubMódulo: Negociaciones (Hereda acceso del Módulo)
  ?   ??? Detalle: Crear (Hereda acceso)
  ?   ??? Detalle: Aprobar (Hereda acceso)
  ??? SubMódulo: Órdenes (Hereda acceso del Módulo)
      ??? Detalle: Ver (Hereda acceso)
      ??? Detalle: Editar (Hereda acceso)

Módulo: Maestros (TieneAcceso = false)
  ??? SubMódulo: Productos (TieneAcceso = true) ? Acceso específico
  ?   ??? Detalle: Ver (Hereda del SubMódulo)
  ?   ??? Detalle: Editar (TieneAcceso = false) ? Sin acceso
  ??? SubMódulo: Vehículos (Sin acceso)
```

## Endpoints Principales:

### 1. Obtener Módulos con Jerarquía
```http
GET /api/permisos/modulos?tipoPlataforma=Web
```
Retorna la estructura completa de módulos organizados jerárquicamente.

### 2. Obtener Permisos de un Rol
```http
GET /api/permisos/rol/{idRol}
```
Retorna todos los permisos del rol aplicando la lógica de herencia.

### 3. Asignar Permiso
```http
POST /api/permisos/asignar
{
  "idRol": 1,
  "idModulo": 2,        // Opcional
  "idSubModulo": null,  // Opcional
  "idSubModuloDetalle": null,  // Opcional
  "tieneAcceso": true
}
```

**Casos de uso:**
- **Acceso a Módulo completo**: Solo enviar `idModulo`
- **Acceso a SubMódulo específico**: Enviar `idModulo` + `idSubModulo`
- **Acceso a Detalle específico**: Enviar los 3 IDs

### 4. Asignar Múltiples Permisos
```http
POST /api/permisos/asignar-multiples
{
  "idRol": 1,
  "permisos": [
    { "idModulo": 1, "tieneAcceso": true },
    { "idModulo": 2, "idSubModulo": 3, "tieneAcceso": true }
  ]
}
```

### 5. Revocar Permiso
```http
DELETE /api/permisos/revocar?idRol=1&idModulo=2
```

## Casos de Uso Implementados:

### Caso 1: Rol de Administrador
```csharp
// Dar acceso completo a todos los módulos
POST /api/permisos/asignar-multiples
{
  "idRol": 1,
  "permisos": [
    { "idModulo": 1, "tieneAcceso": true },  // Compras completo
    { "idModulo": 2, "tieneAcceso": true },  // Maestros completo
    { "idModulo": 3, "tieneAcceso": true }   // Personas completo
  ]
}
```

### Caso 2: Rol de Supervisor (Acceso limitado)
```csharp
// Acceso solo a ciertos SubMódulos
POST /api/permisos/asignar-multiples
{
  "idRol": 2,
  "permisos": [
    { "idModulo": 1, "idSubModulo": 1, "tieneAcceso": true },  // Solo Negociaciones
    { "idModulo": 2, "idSubModulo": 3, "tieneAcceso": true }   // Solo Productos
  ]
}
```

### Caso 3: Rol de Operador (Acceso muy limitado)
```csharp
// Acceso solo a funciones específicas
POST /api/permisos/asignar-multiples
{
  "idRol": 3,
  "permisos": [
    { "idModulo": 1, "idSubModulo": 1, "idSubModuloDetalle": 1, "tieneAcceso": true },  // Solo Ver Negociaciones
    { "idModulo": 2, "idSubModulo": 3, "idSubModuloDetalle": 5, "tieneAcceso": true }   // Solo Consultar Productos
  ]
}
```

## Validaciones:
- Un permiso debe tener al menos un nivel definido (Módulo, SubMódulo o Detalle)
- Los IDs de Rol, Módulo, SubMódulo y Detalle deben existir
- No se permiten permisos duplicados (se actualizan)

## Entidades Relacionadas:
- `PermisoRol`: Tabla intermedia de permisos
- `Rol`: Roles del sistema
- `Modulo`: Módulos de primer nivel
- `SubModulo`: SubMódulos hijos de Módulos
- `SubModuloDetalle`: Detalles específicos de SubMódulos

## DTOs Principales:
- `PermisoRolConJerarquiaDto`: Estructura completa con herencia aplicada
- `ModuloDto`: Información de módulo con sus hijos
- `AsignarPermisoDto`: Para asignar/actualizar permisos
- `AsignarPermisosMultiplesDto`: Para operaciones en batch

## Características Avanzadas:
- ? Herencia automática de permisos
- ? Validación de niveles de acceso
- ? Asignación múltiple de permisos
- ? Consulta optimizada con jerarquía
- ? Revocación de permisos específicos