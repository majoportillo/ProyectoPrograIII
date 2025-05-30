using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalPrograIII.Models
{
    public partial class DetalleVenta
    {
        public int VentaId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public virtual Producto Producto { get; set; } = null!;

        public virtual Venta Venta { get; set; } = null!;
    }
}
