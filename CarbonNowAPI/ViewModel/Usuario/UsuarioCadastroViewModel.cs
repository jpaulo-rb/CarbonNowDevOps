using System.ComponentModel.DataAnnotations;
using CarbonNowAPI.Model;

namespace CarbonNowAPI.ViewModel.Usuario {
    public class UsuarioCadastroViewModel {

        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        [StringLength(64, ErrorMessage = "O nome do usuário não pode exceder 64 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
        [StringLength(128, ErrorMessage = "O nome não pode exceder 128 caracteres")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha do usuário é obrigatória.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha precisa possuir mais de 6 e menos de 20 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A regra do usuário é obrigatória.")]
        public Regra Regra { get; set; }
    }
}