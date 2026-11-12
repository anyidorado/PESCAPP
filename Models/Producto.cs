using System.ComponentModel.DataAnnotations;

namespace PESCAPP.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Categoria { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public decimal CantidadDisponible { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public DateTime FechaPublicacion { get; set; } = DateTime.Now;
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Observaciones { get; set; } = string.Empty;

        [StringLength(200)]
        public string Procedencia { get; set; } = string.Empty;

        public string? VendedorId { get; set; }

        public ApplicationUser? Vendedor { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
