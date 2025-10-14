using System.ComponentModel.DataAnnotations;
using CarbonNowAPI.Model;

namespace CarbonNowAPI.ViewModel.Eletricidade {
    public class EletricidadeViewModel {

        [Required]
        public int Id { get; set; }

        [Required]
        public UnidadeEletricidade UnidadeEletricidade { get; set; }

        [Required]
        public decimal ValorEletricidade { get; set; }

        [Required]
        public DateOnly DataEstimacao { get; set; }

        [Required]
        public decimal CarbonoKg { get; set; }

        [Required]
        public int UsuarioId { get; set; }
    }
}