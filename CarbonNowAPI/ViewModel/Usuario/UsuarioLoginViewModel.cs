using System.ComponentModel.DataAnnotations;

namespace CarbonNowAPI.ViewModel.Usuario {
    public class UsuarioLoginViewModel {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [StringLength(128, ErrorMessage = "O nome não pode exceder 128 caracteres")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha precisa possuir mais de 6 e menos de 20 caracteres")]
        public string Senha { get; set; }

    }
}
