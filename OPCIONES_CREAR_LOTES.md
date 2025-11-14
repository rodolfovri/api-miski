# ?? Opciones para Crear Lotes con Compra

## Contexto
Ahora tienes **3 formas diferentes** de crear y asignar lotes a compras, cada una para un caso de uso específico.

---

## ?? Opción 1: Crear Lote Independiente (POST /api/compras/lotes)

### Endpoint
```http
POST /api/compras/lotes
```

### Request
```json
{
  "peso": 5000.50,
  "sacos": 100,
  "codigo": "LOTE-001",
  "comision": 250.00,
  "observacion": "Lote de quinua blanca"
}
```

### Características
- ? Crea un lote **sin** asignarlo a ninguna compra
- ? El lote queda disponible para ser asignado después
- ? Útil cuando quieres crear varios lotes primero y luego asignarlos

### Cuándo usar
- Cuando tienes lotes que aún no sabes a qué compra asignar
- Cuando quieres un inventario de lotes disponibles
- Cuando el flujo requiere aprobaciones antes de asignar

---

## ?? Opción 2: Crear Lote y Asignar en 1 Paso (POST /api/compras/{idCompra}/lote) ? NUEVO

### Endpoint
```http
POST /api/compras/{idCompra}/lote
```

### Request
```json
{
  "peso": 5000.50,
  "sacos": 100,
  "codigo": "LOTE-001",
  "comision": 250.00,
  "observacion": "Lote de quinua blanca",
  "montoTotal": 42500.00
}
```

### Características
- ? Crea el lote **Y** lo asigna a la compra en **un solo paso**
- ? Más rápido y directo
- ? Actualiza automáticamente el `MontoTotal` de la compra
- ? Valida que la compra no tenga un lote ya asignado (1:1)

### Validaciones
- La compra debe existir
- La compra debe estar en estado ACTIVO
- La compra NO debe tener un lote ya asignado
- El código del lote no debe estar duplicado

### Cuándo usar ? RECOMENDADO
- **Cuando ya sabes a qué compra asignar el lote** (caso más común)
- Cuando quieres simplificar el flujo en el panel web
- Cuando creas la compra y el lote en secuencia

---

## ?? Opción 3: Crear Lote Independiente + Asignar Después (2 pasos)

### Paso 1: Crear lote
```http
POST /api/compras/lotes
{
  "peso": 5000.50,
  "sacos": 100,
  "codigo": "LOTE-001"
}
```

### Paso 2: Asignar a compra
```http
PUT /api/compras/{idCompra}/asignar-lote
{
  "idCompra": 1,
  "idLote": 5,
  "montoTotal": 42500.00
}
```

### Características
- ? Máxima flexibilidad
- ? Puedes crear múltiples lotes primero
- ? Puedes asignar lotes existentes a diferentes compras

### Cuándo usar
- Cuando necesitas revisar/aprobar el lote antes de asignarlo
- Cuando quieres reasignar lotes entre compras
- Cuando el flujo de negocio requiere separación de pasos

---

## ?? Comparación de Opciones

| Característica | Opción 1<br>(POST /lotes) | Opción 2<br>(POST /{id}/lote) ? | Opción 3<br>(2 pasos) |
|----------------|---------------------------|----------------------------------|----------------------|
| **Pasos** | 1 | 1 | 2 |
| **Asignación automática** | ? No | ? Sí | ? No |
| **Requiere IdCompra** | ? No | ? Sí | ? Sí |
| **Requiere MontoTotal** | ? No | ? Sí | ? Sí |
| **Flexibilidad** | Alta | Media | Muy Alta |
| **Velocidad** | - | ? Rápida | Lenta |
| **Complejidad** | Baja | Baja | Media |

---

## ?? Recomendaciones por Caso de Uso

### ? Panel Web - Crear Compra con Lote
**Usar: Opción 2** (POST /api/compras/{idCompra}/lote)
```javascript
// Flujo típico en el panel web
async function crearCompraConLote(idCompra, loteData) {
  // 1 solo request
  const response = await fetch(`/api/compras/${idCompra}/lote`, {
    method: 'POST',
    body: JSON.stringify({
      peso: loteData.peso,
      sacos: loteData.sacos,
      codigo: loteData.codigo,
      montoTotal: loteData.montoTotal
    })
  });
  
  return response.json();
}
```

### ? Gestión de Inventario de Lotes
**Usar: Opción 1** (POST /api/compras/lotes)
```javascript
// Crear lotes sin asignar
async function crearLotesDisponibles(lotes) {
  const promises = lotes.map(lote => 
    fetch('/api/compras/lotes', {
      method: 'POST',
      body: JSON.stringify(lote)
    })
  );
  
  return Promise.all(promises);
}
```

### ? Reasignación de Lotes
**Usar: Opción 3** (2 pasos)
```javascript
// Primero desasignar
await fetch(`/api/compras/${idCompraOrigen}/desasignar-lote`, {
  method: 'PUT'
});

// Luego asignar a otra compra
await fetch(`/api/compras/${idCompraDestino}/asignar-lote`, {
  method: 'PUT',
  body: JSON.stringify({
    idCompra: idCompraDestino,
    idLote: idLote,
    montoTotal: nuevoMontoTotal
  })
});
```

---

## ?? Ejemplos de Uso Completo

### Ejemplo 1: Crear Compra y Lote (FLUJO RÁPIDO) ?

```javascript
// 1. La compra ya fue creada al aprobar la negociación
const idCompra = 1; // Viene del flujo anterior

// 2. Crear lote y asignarlo en 1 paso
const response = await fetch(`/api/compras/${idCompra}/lote`, {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    peso: 5000.50,
    sacos: 100,
    codigo: "LOTE-2024-001",
    comision: 250.00,
    observacion: "Quinua blanca de primera",
    montoTotal: 42500.00
  })
});

const lote = await response.json();
console.log('Lote creado y asignado:', lote);
```

### Ejemplo 2: Gestionar Lotes Disponibles

```javascript
// 1. Crear varios lotes sin asignar
const lotesCreados = [];

for (const loteData of lotesACrear) {
  const response = await fetch('/api/compras/lotes', {
    method: 'POST',
    body: JSON.stringify(loteData)
  });
  
  lotesCreados.push(await response.json());
}

// 2. Listar lotes disponibles (sin asignar)
const response = await fetch('/api/compras/lotes');
const todosLosLotes = await response.json();
const lotesDisponibles = todosLosLotes.data.filter(l => !l.idCompra);

// 3. Asignar un lote disponible cuando sea necesario
await fetch(`/api/compras/${idCompra}/asignar-lote`, {
  method: 'PUT',
  body: JSON.stringify({
    idCompra: idCompra,
    idLote: lotesDisponibles[0].idLote,
    montoTotal: calcularMontoTotal()
  })
});
```

---

## ?? Configuración Recomendada en el Panel Web

### Formulario: Crear Compra con Lote

```html
<form @submit="crearCompraConLote">
  <h3>Datos del Lote</h3>
  
  <input v-model="lote.peso" placeholder="Peso (kg)" required />
  <input v-model="lote.sacos" placeholder="Cantidad de sacos" required />
  <input v-model="lote.codigo" placeholder="Código (opcional)" />
  <input v-model="lote.comision" placeholder="Comisión (opcional)" />
  <textarea v-model="lote.observacion" placeholder="Observaciones"></textarea>
  <input v-model="lote.montoTotal" placeholder="Monto Total" required />
  
  <button type="submit">Crear Lote para Compra</button>
</form>

<script>
methods: {
  async crearCompraConLote() {
    try {
      // Usar la Opción 2: POST /api/compras/{id}/lote
      const response = await this.$http.post(
        `/api/compras/${this.idCompra}/lote`,
        this.lote
      );
      
      this.$toast.success('Lote creado y asignado exitosamente');
      this.$router.push(`/compras/${this.idCompra}`);
    } catch (error) {
      this.$toast.error(error.message);
    }
  }
}
</script>
```

---

## ?? Diseño de UI Sugerido

### Página: Detalle de Compra

```
???????????????????????????????????????????????????????????
? Compra #1 - Estado: ACTIVO                              ?
???????????????????????????????????????????????????????????
? Serie: CMP-2024-001                                      ?
? Negociación: #25                                         ?
? Monto Total: S/. 42,500.00                              ?
???????????????????????????????????????????????????????????
? ?? LOTE ASIGNADO                                        ?
?                                                          ?
? ?? Sin lote asignado ????????????????????????????????? ?
? ?                                                      ? ?
? ? [+ Crear Nuevo Lote]  [?? Asignar Lote Existente] ? ?
? ?                                                      ? ?
? ??????????????????????????????????????????????????????? ?
???????????????????????????????????????????????????????????

Al hacer clic en "+ Crear Nuevo Lote":
- Usa: POST /api/compras/{id}/lote ?

Al hacer clic en "?? Asignar Lote Existente":
- Muestra lista de lotes disponibles
- Usa: PUT /api/compras/{id}/asignar-lote
```

---

## ?? Reglas de Negocio

### ? Crear Lote para Compra (POST /{id}/lote)
1. ? La compra debe existir
2. ? La compra debe estar ACTIVA
3. ? La compra NO debe tener un lote ya asignado
4. ? El código del lote no debe estar duplicado
5. ? Se crea el lote
6. ? Se asigna automáticamente a la compra
7. ? Se actualiza el MontoTotal de la compra

### ? Crear Lote Independiente (POST /lotes)
1. ? No requiere compra
2. ? El código no debe estar duplicado
3. ? Se crea el lote sin asignar
4. ? Queda disponible para asignación posterior

---

## ?? Resumen

**Para tu caso de uso (panel web donde creas compra y lote juntos):**

### ? RECOMENDADO: Usar Opción 2
```
POST /api/compras/{idCompra}/lote
```

**Ventajas:**
- ? 1 solo request en lugar de 2
- ? Más simple en el frontend
- ? Menos código
- ? Más rápido
- ? Menos errores posibles

**Request:**
```json
{
  "peso": 5000.50,
  "sacos": 100,
  "codigo": "LOTE-001",
  "montoTotal": 42500.00
}
```

**Ya NO necesitas:**
- ? POST /api/compras/lotes (sin idCompra)
- ? PUT /api/compras/{id}/asignar-lote

**A menos que:**
- Necesites crear lotes sin asignar (inventario)
- Necesites reasignar lotes entre compras
- Tu flujo requiera aprobaciones intermedias

---

## ?? Endpoints Disponibles

| Método | Endpoint | Uso |
|--------|----------|-----|
| POST | `/api/compras/lotes` | Crear lote sin asignar |
| POST | `/api/compras/{id}/lote` ? | **Crear y asignar en 1 paso** |
| PUT | `/api/compras/{id}/asignar-lote` | Asignar lote existente |
| PUT | `/api/compras/{id}/desasignar-lote` | Desasignar lote |
| GET | `/api/compras/lotes` | Listar todos los lotes |
| GET | `/api/compras/lotes/{id}` | Obtener lote por ID |
| PUT | `/api/compras/lotes/{id}` | Actualizar lote |
| DELETE | `/api/compras/lotes/{id}` | Eliminar lote |

---

**? Compilación exitosa**
**?? Todo listo para usar**
