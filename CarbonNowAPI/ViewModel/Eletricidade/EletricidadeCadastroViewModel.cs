using System.ComponentModel.DataAnnotations;
using CarbonNowAPI.Model;

namespace CarbonNowAPI.ViewModel.Eletricidade {
    public class EletricidadeCadastroViewModel {

        [Required(ErrorMessage = "A unidade de eletricidade é obrigatória.")]
        public UnidadeEletricidade UnidadeEletricidade { get; set; }

        [Required(ErrorMessage = "O valor da eletricidade é obrigatório.")]
        public decimal ValorEletricidade { get; set; }

        [Required(ErrorMessage = "A data da estimação da eletricidade é obrigatória.")]
        public DateOnly DataEstimacao { get; set; }

        [Required(ErrorMessage = "O valor da emissão de Carbono é obrigatório.")]
        public decimal CarbonoKg { get; set; }
    }
}