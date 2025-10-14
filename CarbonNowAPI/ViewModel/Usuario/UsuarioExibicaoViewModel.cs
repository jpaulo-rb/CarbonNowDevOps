using System.ComponentModel.DataAnnotations;
using CarbonNowAPI.Model;

namespace CarbonNowAPI.ViewModel.Usuario {
    public class UsuarioExibicaoViewModel {
        public int Id { get; set; }
        public string Nome { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public Regra Regra { get; set; }
    }
}
