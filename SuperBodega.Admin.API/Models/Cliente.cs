using SuperBodega.Models;
using System;
using System.Collections.Generic;

namespace SuperBodega.Admin.API.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Direccion { get; set; }

        public string? Email { get; set; }

        public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

        public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
    }
}
