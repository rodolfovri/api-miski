# ?? GUÍA DE TRACKING - Sistema de Ubicación en Tiempo Real

## ?? Flujo Completo del Sistema

### **1?? LOGIN - Registro de Dispositivo (Mobile)**

Cuando un usuario de **Mobile** hace login, se registra automáticamente su dispositivo:

```json
POST /api/auth/login
{
  "numeroDocumento": "12345678",
  "password": "mipassword",
  "tipoPlataforma": "Mobile",  // ? Activa el registro de dispositivo
  
  // Campos adicionales para Mobile
  "deviceId": "550e8400-e29b-41d4-a716-446655440000",
  "modeloDispositivo": "Samsung Galaxy S23",
  "sistemaOperativo": "Android 14",
  "versionApp": "1.0.0"
}
```

**Respuesta:**
```json
{
  "success": true,
  "data": {
    "idUsuario": 1,
    "username": "jperez",
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "expiration": "2025-01-24T10:00:00Z",
    "persona": { ... },
    "roles": [ ... ]
  }
}
```

**¿Qué sucede internamente?**
- ? Se valida usuario y contraseña
- ? Si `tipoPlataforma == "Mobile"`:
  - Si el `DeviceId` NO existe ? Se crea un nuevo dispositivo
  - Si el `DeviceId` YA existe ? Se actualiza `FUltimaActividad` y `VersionApp`
- ? Se genera el JWT token
- ? Se retornan roles y permisos filtrados por plataforma

---

## **2?? TRACKING SIN AUTENTICACIÓN (Background)**

### **Endpoint:** `POST /api/tracking/registrar-ubicacion-dispositivo`

**?? NO REQUIERE `[Authorize]`**

**Uso:** Tracking en segundo plano cuando:
- La app está cerrada
- El usuario no tiene sesión activa
- El tracking continúa automáticamente

**Request:**
```json
POST /api/tracking/registrar-ubicacion-dispositivo
{
  "deviceId": "550e8400-e29b-41d4-a716-446655440000",
  "idPersona": 123,
  "latitud": "-6.7191234",
  "longitud": "-79.9065123",
  "precision": 15.5,
  "velocidad": 45.0
}
```

**Validaciones de seguridad:**
1. ? El `DeviceId` debe existir en la base de datos
2. ? El `DeviceId` debe estar activo (`Activo = true`)
3. ? El `IdPersona` debe coincidir con el dispositivo registrado

**Respuesta:**
```json
{
  "success": true,
  "message": "Ubicación registrada exitosamente"
}
```

**¿Qué sucede internamente?**
- ? Se valida que el dispositivo existe y está activo
- ? Se valida que el `IdPersona` coincida (seguridad)
- ? Se desactiva la ubicación actual anterior (`EsActual = false`)
- ? Se crea un nuevo registro de tracking con `EsActual = true`
- ? Se actualiza `FUltimaActividad` del dispositivo
- ?? (Opcional) Se envía notificación en tiempo real vía SignalR

---

## **3?? TRACKING CON AUTENTICACIÓN (Usuario Activo)**

### **Endpoint:** `POST /api/tracking/registrar-ubicacion`

**? REQUIERE `[Authorize]`**

**Uso:** Tracking cuando el usuario está usando activamente la app

**Request:**
```json
POST /api/tracking/registrar-ubicacion
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...

{
  "latitud": "-6.7191234",
  "longitud": "-79.9065123",
  "precision": 15.5,
  "velocidad": 45.0
}
```

**Nota:** El `IdPersona` se obtiene automáticamente del JWT token.

**Respuesta:**
```json
{
  "success": true,
  "message": "Ubicación registrada exitosamente"
}
```

---

## **4?? CONSULTAS DE TRACKING**

### **A. Obtener ubicación actual de una persona**

```http
GET /api/tracking/ubicacion-actual/123
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

**Respuesta:**
```json
{
  "success": true,
  "data": {
    "idTracking": 456,
    "idPersona": 123,
    "nombreCompleto": "Juan Pérez García",
    "numeroDocumento": "12345678",
    "latitud": "-6.7191234",
    "longitud": "-79.9065123",
    "precision": 15.5,
    "velocidad": 45.0,
    "fRegistro": "2025-01-23T14:30:45",
    "esActual": true,
    "estado": "ACTIVO"
  }
}
```

---

### **B. Obtener mi ubicación actual (usuario autenticado)**

```http
GET /api/tracking/mi-ubicacion
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

**Respuesta:** Igual que la anterior, pero del usuario logueado.

---

### **C. Obtener historial de ubicaciones**

```http
GET /api/tracking/historial/123?fechaInicio=2025-01-01&fechaFin=2025-01-23&limite=50
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

**Respuesta:**
```json
{
  "success": true,
  "data": [
    {
      "idTracking": 456,
      "idPersona": 123,
      "nombreCompleto": "Juan Pérez García",
      "latitud": "-6.7191234",
      "longitud": "-79.9065123",
      "fRegistro": "2025-01-23T14:30:45",
      "esActual": false,
      "estado": "ACTIVO"
    },
    // ... más registros
  ],
  "message": "50 ubicaciones encontradas"
}
```

---

### **D. Obtener todas las ubicaciones activas (Mapa en tiempo real)**

```http
GET /api/tracking/ubicaciones-activas
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

**Uso:** Mostrar en un mapa todas las posiciones actuales de todas las personas.

**Respuesta:**
```json
{
  "success": true,
  "data": [
    {
      "idTracking": 456,
      "idPersona": 123,
      "nombreCompleto": "Juan Pérez García",
      "latitud": "-6.7191234",
      "longitud": "-79.9065123",
      "fRegistro": "2025-01-23T14:30:45",
      "esActual": true
    },
    {
      "idTracking": 789,
      "idPersona": 124,
      "nombreCompleto": "María López",
      "latitud": "-6.7201234",
      "longitud": "-79.9075123",
      "fRegistro": "2025-01-23T14:31:00",
      "esActual": true
    }
  ],
  "message": "2 ubicaciones activas"
}
```

---

## **?? Diagrama de Flujo**

```
???????????????????????????????????????????????????????????????
?  MOBILE APP                                                 ?
???????????????????????????????????????????????????????????????
?                                                             ?
?  1. Usuario hace LOGIN                                      ?
?     ?? Envía: DeviceId, ModeloDispositivo, etc.           ?
?     ?? Recibe: JWT Token                                   ?
?                                                             ?
?  2. App detecta ubicación cada X minutos                    ?
?     ?? Si usuario está ACTIVO (con sesión):                ?
?     ?   ?? POST /tracking/registrar-ubicacion (con JWT)    ?
?     ?                                                       ?
?     ?? Si app está CERRADA (sin sesión):                   ?
?         ?? POST /tracking/registrar-ubicacion-dispositivo  ?
?            (sin JWT, solo DeviceId)                        ?
?                                                             ?
???????????????????????????????????????????????????????????????
                           ?
                           ?
???????????????????????????????????????????????????????????????
?  API BACKEND (.NET)                                         ?
???????????????????????????????????????????????????????????????
?                                                             ?
?  ? Valida DeviceId (si es background)                      ?
?  ? Valida JWT (si es autenticado)                         ?
?  ? Desactiva ubicación anterior (EsActual = false)        ?
?  ? Crea nuevo registro (EsActual = true)                  ?
?  ? Actualiza FUltimaActividad del dispositivo             ?
?  ?? (Opcional) Notifica vía SignalR                        ?
?                                                             ?
???????????????????????????????????????????????????????????????
                           ?
                           ?
???????????????????????????????????????????????????????????????
?  WEB DASHBOARD                                              ?
???????????????????????????????????????????????????????????????
?                                                             ?
?  ?? Mapa en tiempo real                                     ?
?     ?? GET /tracking/ubicaciones-activas                   ?
?                                                             ?
?  ?? Historial de rutas                                      ?
?     ?? GET /tracking/historial/{idPersona}                 ?
?                                                             ?
???????????????????????????????????????????????????????????????
```

---

## **??? Estructura de Archivos Creados**

```
Miski.Api/
??? Controllers/
?   ??? TrackingController.cs                    ? Endpoints REST

Miski.Application/
??? Features/
?   ??? Tracking/
?       ??? Commands/
?       ?   ??? RegistrarUbicacion/
?       ?   ?   ??? RegistrarUbicacionCommand.cs
?       ?   ?   ??? RegistrarUbicacionHandler.cs
?       ?   ?   ??? RegistrarUbicacionValidator.cs
?       ?   ?
?       ?   ??? RegistrarUbicacionDispositivo/
?       ?       ??? RegistrarUbicacionDispositivoCommand.cs
?       ?       ??? RegistrarUbicacionDispositivoHandler.cs
?       ?       ??? RegistrarUbicacionDispositivoValidator.cs
?       ?
?       ??? Queries/
?           ??? GetUbicacionActual/
?           ?   ??? GetUbicacionActualQuery.cs
?           ?   ??? GetUbicacionActualHandler.cs
?           ?
?           ??? GetHistorialUbicaciones/
?           ?   ??? GetHistorialUbicacionesQuery.cs
?           ?   ??? GetHistorialUbicacionesHandler.cs
?           ?
?           ??? GetUbicacionesActivas/
?               ??? GetUbicacionesActivasQuery.cs
?               ??? GetUbicacionesActivasHandler.cs

Miski.Shared/
??? DTOs/
    ??? Tracking/
        ??? UbicacionDispositivoDto.cs           ? Para background
        ??? RegistrarUbicacionDto.cs             ? Para autenticado
        ??? TrackingResponseDto.cs               ? Respuesta

Miski.Domain/
??? Entities/
    ??? DispositivoPersona.cs                    ? Dispositivos
    ??? TrackingPersona.cs                       ? Ubicaciones
```

---

## **?? Seguridad**

### **Endpoint sin autenticación (`registrar-ubicacion-dispositivo`)**

**Validaciones:**
1. ? El `DeviceId` debe existir en la BD (se registró durante el login)
2. ? El `DeviceId` debe estar activo
3. ? El `IdPersona` debe coincidir con el dispositivo

**Esto previene:**
- ? Registros falsos de ubicación
- ? Suplantación de identidad
- ? Spam de ubicaciones

### **Endpoint con autenticación (`registrar-ubicacion`)**

- ? Requiere JWT válido
- ? El `IdPersona` se extrae del token (no puede ser falsificado)

---

## **? CHECKLIST DE IMPLEMENTACIÓN**

- [x] LoginDto con campos de dispositivo
- [x] LoginHandler registra dispositivo en Mobile
- [x] DTOs de tracking creados
- [x] Command: RegistrarUbicacionDispositivo (sin JWT)
- [x] Command: RegistrarUbicacion (con JWT)
- [x] Query: GetUbicacionActual
- [x] Query: GetHistorialUbicaciones
- [x] Query: GetUbicacionesActivas
- [x] Controller: TrackingController
- [ ] Configurar SignalR (opcional - tiempo real)
- [ ] Pruebas unitarias
- [ ] Documentación Swagger

---

## **?? PRÓXIMOS PASOS**

1. **Probar los endpoints** desde Swagger
2. **Implementar SignalR** para notificaciones en tiempo real (opcional)
3. **Optimizar queries** con repositorios especializados si hay performance issues
4. **Agregar índices** en la BD para `EsActual`, `IdPersona`, `FRegistro`

---

¡Listo! ?? El sistema de tracking está completamente implementado.
