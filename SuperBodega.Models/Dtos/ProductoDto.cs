﻿namespace SuperBodega.Admin.API.Dtos
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int? Stock { get; set; }
        public int? ProveedorId { get; set; }
    }
}
