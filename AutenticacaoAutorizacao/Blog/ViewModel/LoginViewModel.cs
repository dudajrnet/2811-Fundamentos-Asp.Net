using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel
{
    public class LoginViewModel
    {       
        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O E-mail é inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; } = string.Empty;
    }
}
