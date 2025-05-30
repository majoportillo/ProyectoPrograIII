using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalPrograIII.Models
{
    public partial class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Correo { get; set; }

        public string? Telefono { get; set; }

        public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public virtual ICollection<Carrito> Carrito { get; set; } = new List<Carrito>();

    }
}
