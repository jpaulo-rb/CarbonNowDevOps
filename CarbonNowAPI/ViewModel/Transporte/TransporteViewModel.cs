using System.ComponentModel.DataAnnotations;
using CarbonNowAPI.Model;

namespace CarbonNowAPI.ViewModel.Transporte {
    public class TransporteViewModel {

        [Required]
        public int Id { get; set; }

        [Required]
        public decimal ValorPesoKg { get; set; }

        [Required]
        public decimal ValorDistanciaKm { get; set; }

        [Required]
        public MetodoTransporte MetodoTransporte { get; set; }

        [Required]
        public DateOnly DataEstimacao { get; set; }

        [Required]
        public decimal CarbonoKg { get; set; }

        [Required]
        public int UsuarioId { get; set; }
    }
}