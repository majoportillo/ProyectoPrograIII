using System;
using System.Collections.Generic;

namespace SuperBodega.Models;

public partial class Compra
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public int? ProveedorId { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedores? Proveedor { get; set; }
}
