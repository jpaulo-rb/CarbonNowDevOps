using System.ComponentModel.DataAnnotations;
using CarbonNowAPI.Model;

namespace CarbonNowAPI.ViewModel.Usuario {
    public class UsuarioAlteracaoViewModel {

        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        public int Id { get; set; }

        [StringLength(64, ErrorMessage = "O nome do usuário não pode exceder 64 caracteres")]
        public string Nome { get; set; }

        [StringLength(128, ErrorMessage = "O E-mail do usuário não pode exceder 128 caracteres")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        public string Email { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha do usuário precisa possuir mais de 6 e menos de 20 caracteres")]
        public string Senha { get; set; }

        public Regra Regra { get; set; }

    }
}
