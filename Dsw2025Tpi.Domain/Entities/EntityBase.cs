namespace Dsw2025Tpi.Domain.Entities;

public abstract class EntityBase
{
    protected EntityBase()
    {
        Id = Guid.NewGuid();
    }

    // El constructor ejecuta Guid.NewGuid(), pero ese valor se
    // descarta inmediatamente porque luego System.Text.Json lo pisa con el valor del archivo.

    public Guid Id { get; set; }
}

