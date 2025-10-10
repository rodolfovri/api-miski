# M�dulo de Permisos

Este m�dulo gestiona el sistema de permisos jer�rquico con 3 niveles: M�dulo ? SubM�dulo ? SubM�duloDetalle.

## Funcionalidades principales:
- Gesti�n de permisos por rol con herencia jer�rquica
- Estructura de 3 niveles: M�dulo, SubM�dulo y SubM�duloDetalle
- Asignaci�n y revocaci�n de permisos
- Consulta de permisos con jerarqu�a completa

## Controladores:
- `PermisosController`: Gesti�n completa de permisos por rol

## L�gica de Permisos

### Herencia Jer�rquica:
- **Si un rol tiene acceso a un M�dulo**: Autom�ticamente accede a todos sus SubM�dulos y SubM�duloDetalles
- **Si tiene acceso solo a un SubM�dulo**: Solo accede a ese SubM�dulo y sus SubM�duloDetalles
- **Si tiene acceso solo a un SubM�duloDetalle**: Acceso limitado �nicamente a ese detalle

### Ejemplo:
```
M�dulo: Compras (TieneAcceso = true)
  ??? SubM�dulo: Negociaciones (Hereda acceso del M�dulo)
  ?   ??? Detalle: Crear (Hereda acceso)
  ?   ??? Detalle: Aprobar (Hereda acceso)
  ??? SubM�dulo: �rdenes (Hereda acceso del M�dulo)
      ??? Detalle: Ver (Hereda acceso)
      ??? Detalle: Editar (Hereda acceso)

M�dulo: Maestros (TieneAcceso = false)
  ??? SubM�dulo: Productos (TieneAcceso = true) ? Acceso espec�fico
  ?   ??? Detalle: Ver (Hereda del SubM�dulo)
  ?   ??? Detalle: Editar (TieneAcceso = false) ? Sin acceso
  ??? SubM�dulo: Veh�culos (Sin acceso)
```

## Endpoints Principales:

### 1. Obtener M�dulos con Jerarqu�a
```http
GET /api/permisos/modulos?tipoPlataforma=Web
```
Retorna la estructura completa de m�dulos organizados jer�rquicamente.

### 2. Obtener Permisos de un Rol
```http
GET /api/permisos/rol/{idRol}
```
Retorna todos los permisos del rol aplicando la l�gica de herencia.

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
- **Acceso a M�dulo completo**: Solo enviar `idModulo`
- **Acceso a SubM�dulo espec�fico**: Enviar `idModulo` + `idSubModulo`
- **Acceso a Detalle espec�fico**: Enviar los 3 IDs

### 4. Asignar M�ltiples Permisos
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
// Dar acceso completo a todos los m�dulos
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
// Acceso solo a ciertos SubM�dulos
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
// Acceso solo a funciones espec�ficas
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
- Un permiso debe tener al menos un nivel definido (M�dulo, SubM�dulo o Detalle)
- Los IDs de Rol, M�dulo, SubM�dulo y Detalle deben existir
- No se permiten permisos duplicados (se actualizan)

## Entidades Relacionadas:
- `PermisoRol`: Tabla intermedia de permisos
- `Rol`: Roles del sistema
- `Modulo`: M�dulos de primer nivel
- `SubModulo`: SubM�dulos hijos de M�dulos
- `SubModuloDetalle`: Detalles espec�ficos de SubM�dulos

## DTOs Principales:
- `PermisoRolConJerarquiaDto`: Estructura completa con herencia aplicada
- `ModuloDto`: Informaci�n de m�dulo con sus hijos
- `AsignarPermisoDto`: Para asignar/actualizar permisos
- `AsignarPermisosMultiplesDto`: Para operaciones en batch

## Caracter�sticas Avanzadas:
- ? Herencia autom�tica de permisos
- ? Validaci�n de niveles de acceso
- ? Asignaci�n m�ltiple de permisos
- ? Consulta optimizada con jerarqu�a
- ? Revocaci�n de permisos espec�ficos