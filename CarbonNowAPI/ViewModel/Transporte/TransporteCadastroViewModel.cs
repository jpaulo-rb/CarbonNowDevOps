using System.ComponentModel.DataAnnotations;
using CarbonNowAPI.Model;

namespace CarbonNowAPI.ViewModel.Transporte {
    public class TransporteCadastroViewModel {


        [Required(ErrorMessage ="Peso da carga é obrigatório.")]
        public decimal ValorPesoKg { get; set; }

        [Required(ErrorMessage = "Distância do transporte é obrigatório.")]
        public decimal ValorDistanciaKm { get; set; }

        [Required(ErrorMessage = "Método de transporte é obrigatório.")]
        public MetodoTransporte MetodoTransporte { get; set; }

        [Required(ErrorMessage = "Data da estimação é obrigatória.")]
        public DateOnly DataEstimacao { get; set; }

        [Required(ErrorMessage = "O valor da emissão de Carbono é obrigatório.")]
        public decimal CarbonoKg { get; set; }

    }
}