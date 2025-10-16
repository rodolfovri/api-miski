# Gu�a de Configuraci�n y Pruebas - M�dulo Negociaciones

## ?? Checklist de Configuraci�n

### 1. ? Crear Directorio de Almacenamiento

```powershell
# En Windows (PowerShell)
New-Item -Path "C:\MiskiFiles" -ItemType Directory -Force
New-Item -Path "C:\MiskiFiles\negociaciones" -ItemType Directory -Force
New-Item -Path "C:\MiskiFiles\negociaciones\calidad" -ItemType Directory -Force
New-Item -Path "C:\MiskiFiles\negociaciones\dni" -ItemType Directory -Force
```

```bash
# En Linux/Mac
mkdir -p /var/www/MiskiFiles/negociaciones/calidad
mkdir -p /var/www/MiskiFiles/negociaciones/dni

# Dar permisos de escritura
chmod -R 755 /var/www/MiskiFiles
```

### 2. ? Verificar Registro en Program.cs

El servicio ya est� registrado en `Program.cs`:

```csharp
// File Storage Service
builder.Services.AddScoped<Miski.Application.Services.IFileStorageService, 
                          Miski.Application.Services.LocalFileStorageService>();
```

### 3. ? Verificar Cadena de Conexi�n

En `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=MiskiDB;..."
  }
}
```

### 4. ? Aplicar Migraciones (si es necesario)

```powershell
dotnet ef database update
```

---

## ?? Gu�a de Pruebas

### Herramientas Recomendadas
- **Postman** (Recomendado para multipart/form-data)
- **Swagger UI** (http://localhost:5000/swagger)
- **Thunder Client** (VS Code Extension)

---

## ?? Ejemplos de Pruebas con Postman

### 1. Login (Obtener Token)

```http
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin123!"
}
```

**Respuesta:**
```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiration": "2024-01-15T10:00:00Z"
  }
}
```

**Copiar el token para usar en las siguientes peticiones.**

---

### 2. Crear Negociaci�n (Con Fotos)

**Configuraci�n en Postman:**

1. **M�todo**: POST
2. **URL**: `http://localhost:5000/api/compras/negociaciones`
3. **Headers**:
   ```
   Authorization: Bearer {TU_TOKEN_AQUI}
   ```
4. **Body** ? seleccionar **form-data**:

| Key | Type | Value |
|-----|------|-------|
| idProveedor | Text | 1 |
| idComisionista | Text | 2 |
| idProducto | Text | 1 |
| pesoTotal | Text | 1500.50 |
| sacosTotales | Text | 60 |
| precioUnitario | Text | 8.50 |
| nroCuentaRuc | Text | 12345678901 |
| fotoCalidadProducto | File | [Seleccionar imagen] |
| fotoDniFrontal | File | [Seleccionar imagen] |
| fotoDniPosterior | File | [Seleccionar imagen] |
| observacion | Text | Producto de alta calidad |
| estado | Text | ACTIVO |

**Respuesta Esperada:**
```json
{
  "success": true,
  "message": "Negociaci�n creada exitosamente",
  "data": {
    "idNegociacion": 1,
    "idProveedor": 1,
    "idComisionista": 2,
    "idProducto": 1,
    "pesoTotal": 1500.50,
    "sacosTotales": 60,
    "precioUnitario": 8.50,
    "nroCuentaRuc": "12345678901",
    "fotoCalidadProducto": "/negociaciones/calidad/a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg",
    "fotoDniFrontal": "/negociaciones/dni/b2c3d4e5-f6a7-8901-bcde-f12345678901.jpg",
    "fotoDniPosterior": "/negociaciones/dni/c3d4e5f6-a7b8-9012-cdef-123456789012.jpg",
    "estadoAprobado": "PENDIENTE",
    "estado": "ACTIVO",
    "fRegistro": "2024-01-15T08:30:00Z",
    "proveedorNombre": "Juan P�rez",
    "comisionistaNombre": "Mar�a Gonz�lez",
    "productoNombre": "Caf� Ar�bica",
    "montoTotal": 12754.25
  }
}
```

---

### 3. Obtener Todas las Negociaciones

```http
GET http://localhost:5000/api/compras/negociaciones
Authorization: Bearer {TU_TOKEN}
```

**Con Filtros:**
```http
GET http://localhost:5000/api/compras/negociaciones?estadoAprobado=PENDIENTE&idComisionista=2
Authorization: Bearer {TU_TOKEN}
```

---

### 4. Obtener Negociaci�n por ID

```http
GET http://localhost:5000/api/compras/negociaciones/1
Authorization: Bearer {TU_TOKEN}
```

---

### 5. Actualizar Negociaci�n

**Configuraci�n en Postman:**

1. **M�todo**: PUT
2. **URL**: `http://localhost:5000/api/compras/negociaciones/1`
3. **Headers**:
   ```
   Authorization: Bearer {TU_TOKEN_AQUI}
   ```
4. **Body** ? seleccionar **form-data**:

| Key | Type | Value |
|-----|------|-------|
| idNegociacion | Text | 1 |
| idProveedor | Text | 1 |
| idComisionista | Text | 2 |
| idProducto | Text | 1 |
| pesoTotal | Text | 1600.00 |
| sacosTotales | Text | 65 |
| precioUnitario | Text | 8.75 |
| nroCuentaRuc | Text | 12345678901 |
| fotoCalidadProducto | File | [Opcional - solo si quieres cambiar] |
| observacion | Text | Actualizaci�n de pesos |
| estado | Text | ACTIVO |

---

### 6. Aprobar Negociaci�n

```http
PUT http://localhost:5000/api/compras/negociaciones/1/aprobar
Authorization: Bearer {TU_TOKEN}
Content-Type: application/json

{
  "idNegociacion": 1,
  "aprobadaPor": 3,
  "observacion": "Aprobado - Producto de buena calidad"
}
```

---

### 7. Obtener Pendientes de Aprobaci�n

```http
GET http://localhost:5000/api/compras/negociaciones/pendientes-aprobacion
Authorization: Bearer {TU_TOKEN}
```

---

### 8. Eliminar Negociaci�n

```http
DELETE http://localhost:5000/api/compras/negociaciones/1
Authorization: Bearer {TU_TOKEN}
```

---

## ?? Verificaciones Post-Prueba

### 1. Verificar Archivos en Disco

```powershell
# Windows PowerShell
Get-ChildItem -Path "C:\MiskiFiles\negociaciones\" -Recurse
```

```bash
# Linux/Mac
ls -R /var/www/MiskiFiles/negociaciones/
```

**Deber�as ver:**
```
C:\MiskiFiles\negociaciones\
??? calidad\
?   ??? a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg
??? dni\
    ??? b2c3d4e5-f6a7-8901-bcde-f12345678901.jpg
    ??? c3d4e5f6-a7b8-9012-cdef-123456789012.jpg
```

### 2. Verificar en Base de Datos

```sql
-- Ver negociaciones creadas
SELECT * FROM Negociacion;

-- Ver URLs de fotos
SELECT 
    IdNegociacion,
    FotoCalidadProducto,
    FotoDniFrontal,
    FotoDniPosterior,
    EstadoAprobado
FROM Negociacion;
```

---

## ? Soluci�n de Problemas Comunes

### Error: "Directorio no existe"
```
Soluci�n: Crear manualmente C:\MiskiFiles\ con permisos de escritura
```

### Error: "The ConnectionString property has not been initialized"
```
Soluci�n: Verificar appsettings.json y la cadena de conexi�n
```

### Error: 401 Unauthorized
```
Soluci�n: Verificar que el token JWT est� en el header Authorization
```

### Error: "Cannot access a disposed object"
```
Soluci�n: Verificar que IFileStorageService est� registrado como Scoped en Program.cs
```

### Error: Fotos no se guardan
```
Soluci�n: 
1. Verificar permisos del directorio C:\MiskiFiles\
2. Ejecutar Visual Studio como Administrador
3. Verificar que no hay antivirus bloqueando
```

---

## ?? Casos de Prueba Recomendados

### Pruebas Positivas ?
- [ ] Crear negociaci�n con los 3 archivos
- [ ] Listar negociaciones sin filtros
- [ ] Listar negociaciones con filtros
- [ ] Obtener negociaci�n por ID v�lido
- [ ] Actualizar negociaci�n pendiente (con fotos nuevas)
- [ ] Actualizar negociaci�n pendiente (sin cambiar fotos)
- [ ] Aprobar negociaci�n pendiente
- [ ] Listar pendientes de aprobaci�n
- [ ] Eliminar negociaci�n sin compras

### Pruebas Negativas ?
- [ ] Crear sin fotos ? Debe fallar (400)
- [ ] Crear con peso negativo ? Debe fallar (400)
- [ ] Crear con relaci�n peso/sacos incorrecta ? Debe fallar (400)
- [ ] Crear con comisionista inexistente ? Debe fallar (404)
- [ ] Obtener por ID inexistente ? Debe fallar (404)
- [ ] Actualizar negociaci�n aprobada ? Debe fallar (400)
- [ ] Aprobar negociaci�n ya aprobada ? Debe fallar (400)
- [ ] Eliminar negociaci�n con compras ? Debe fallar (400)

---

## ?? Pr�ximos Pasos

1. **Probar todos los endpoints** siguiendo esta gu�a
2. **Verificar archivos en disco** despu�s de cada operaci�n
3. **Revisar logs** en caso de errores
4. **Documentar problemas** encontrados
5. **Preparar migraci�n** a almacenamiento en la nube

---

## ?? Soporte

Si encuentras problemas:
1. Revisar logs de la aplicaci�n
2. Verificar permisos de carpetas
3. Comprobar cadena de conexi�n
4. Revisar token JWT v�lido

---

**�ltima actualizaci�n**: Enero 2024  
**Estado**: ? Listo para pruebas
