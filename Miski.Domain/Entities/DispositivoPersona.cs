namespace Miski.Domain.Entities;

public class DispositivoPersona
{
    public int IdDispositivoPersona { get; set; }
    /// FK: Referencia a la persona propietaria del dispositivo
    /// Ejemplo: 123 (a qué persona pertenece este celular)
    /// Relación: Muchos dispositivos → Una persona
    /// IMPORTANTE: Un usuario puede tener múltiples dispositivos
    ///   - Teléfono personal
    ///   - Tablet
    ///   - Otro teléfono
    public int IdPersona { get; set; }
    /// Identificador único del dispositivo (UUID)
    /// Se genera automáticamente en cada instalación de la app
    /// Se almacena localmente en el dispositivo (SharedPreferences/Keychain)
    /// Persiste incluso aunque: se cierre sesión, se desinstale app, cambien contraseña
    /// Máximo 255 caracteres (UUID típicamente ~36 caracteres)
    /// Ejemplo: "550e8400-e29b-41d4-a716-446655440000"
    /// ÚNICO EN LA BD: No puede haber dos dispositivos con el mismo UUID
    /// Uso: Identificar requests de tracking SIN JWT
    public string DeviceId { get; set; } = string.Empty;
    /// Modelo del dispositivo para identificación administrativa
    /// Se obtiene del SO
    /// Máximo 100 caracteres
    /// Ejemplo: "Samsung Galaxy S23", "iPhone 15 Pro", "Xiaomi Redmi Note 12"
    /// Uso: Admin puede ver qué modelos usan sus operarios
    public string ModeloDispositivo { get; set; } = string.Empty;
    /// Sistema operativo y versión
    /// Máximo 50 caracteres
    /// Ejemplo: "Android 14", "iOS 17.2", "Android 13"
    /// Uso: Detectar compatibilidad, deprecar versiones antiguas
    public string SistemaOperativo { get; set; } = string.Empty;
    /// Versión de la app Miski instalada
    /// Máximo 20 caracteres
    /// Ejemplo: "1.0.0", "1.2.5", "2.0.0-beta"
    /// Uso: Admin puede ver qué versión usa cada operario
    ///      Forzar actualización si hay bug crítico
    public string VersionApp { get; set; } = string.Empty;
    /// Fecha cuando el dispositivo fue registrado por primera vez
    /// Se establece automáticamente al hacer primer login
    /// NUNCA cambia
    /// Ejemplo: 2025-11-23 10:00:00
    /// Uso: Auditoría, saber cuándo empezó a usar la app
    public DateTime FRegistro { get; set; }
    /// Última fecha en que el dispositivo envió una actualización de ubicación
    /// Se actualiza CADA VEZ que manda tracking (con o sin sesión)
    /// Ejemplo: 2025-11-23 14:45:30
    /// Uso: Detectar dispositivos inactivos/dormidos
    ///      "Lleva 2 horas sin enviar ubicación = algo está mal"
    public DateTime? FUltimaActividad { get; set; }
    /// Flag de activación del dispositivo
    /// Valores: true (puede enviar tracking), false (bloqueado/desactivado)
    /// Ejemplo: true = dispositivo activo y enviando ubicaciones
    ///          false = dispositivo desactivado por usuario o admin
    /// Uso: 
    ///   - Usuario puede desactivar un dispositivo en configuración
    ///   - Admin puede bloquear dispositivos sospechosos
    ///   - Cuando desactivas, el tracking se detiene automáticamente
    public bool Activo { get; set; }

    // Navigation properties
    public virtual Persona Persona { get; set; } = null!;

}
