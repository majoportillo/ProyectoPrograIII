using System;
using System.Collections.Generic;

namespace SuperBodega.Models;

public partial class DetalleVenta
{
    public int VentaId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Venta Venta { get; set; } = null!;
}
