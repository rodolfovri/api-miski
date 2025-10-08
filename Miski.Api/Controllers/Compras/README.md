# M�dulo de Compras

Este m�dulo gestiona todo el proceso de compras, desde las negociaciones hasta la llegada a planta.

## Funcionalidades principales:
- Gesti�n de negociaciones con proveedores
- Creaci�n y seguimiento de compras
- Manejo de lotes de productos
- Procesamiento de llegadas a planta
- Control de veh�culos en las compras

## Controladores:
- `NegociacionesController`: CRUD de negociaciones
- `ComprasController`: CRUD de compras
- `LotesController`: CRUD de lotes
- `LlegadaPlantaController`: Procesamiento de llegadas a planta

## Casos de uso por implementar:
- Crear/actualizar/eliminar negociaciones
- Aprobar/rechazar negociaciones
- Crear compras a partir de negociaciones
- Gestionar lotes por compra
- Registrar llegadas a planta
- Procesar lotes en planta

## Entidades relacionadas:
- `Negociacion`
- `Compra`
- `Lote`
- `LlegadaPlanta`
- `LlegadaPlantaDetalle`
- `CompraVehiculo`