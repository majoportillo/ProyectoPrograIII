using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalPrograIII.Models
{
    public partial class Carrito
    {
        public int Id { get; set; }

        public int? ClienteId { get; set; }

        public int? ProductoId { get; set; }

        public int Cantidad { get; set; }

        public DateTime? FechaAgregado { get; set; }

        public virtual Cliente? Cliente { get; set; }

        public virtual Producto? Producto { get; set; }
    }
}
