namespace Miski.Domain.Entities;

public class MovimientoAlmacen
{
    public int IdMovimientoAlmacen { get; set; }
    public int IdTipoMovimiento { get; set; }
    public int IdUbicacion { get; set; }
    public string TipoStock { get; set; } = null!;
    public int? IdLlegadaPlanta { get; set; }

    // Auditoría
    public int IdUsuario { get; set; }
    public DateTime FRegistro { get; set; }
    public string? Observaciones { get; set; }
    public string Estado { get; set; } = null!;
    public int? IdUsuarioAnulacion { get; set; }
    public DateTime? FAnulacion { get; set; }
    public string? MotivoAnulacion { get; set; }

    // Navegation properties
    public virtual TipoMovimiento TipoMovimiento { get; set; } = null!;
    public virtual Ubicacion Ubicacion { get; set; } = null!;
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Usuario? UsuarioAnulacion { get; set; }
    public virtual LlegadaPlanta? LlegadaPlanta { get; set; }
    public virtual ICollection<DetalleMovimientoAlmacen> DetallesMovimientoAlmacen { get; set; } = new List<DetalleMovimientoAlmacen>();

}
