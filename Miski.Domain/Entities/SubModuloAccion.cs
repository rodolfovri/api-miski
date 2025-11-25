namespace Miski.Domain.Entities;

public class SubModuloAccion
{
    public int IdSubModuloAccion { get; set; }
    public int IdSubModulo { get; set; }
    public int IdAccion { get; set; }
    public bool Habilitado { get; set; }

    // Navigation properties
    public virtual SubModulo SubModulo { get; set; } = null!;
    public virtual Accion Accion { get; set; } = null!;
}
