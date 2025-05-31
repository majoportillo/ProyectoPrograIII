using SuperBodega.Models;
using System;
using System.Collections.Generic;

namespace SuperBodega.Models;

public partial class Venta
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public int? ClienteId { get; set; }

    public decimal? Total { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();
}
