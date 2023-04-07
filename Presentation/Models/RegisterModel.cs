using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class RegisterModel
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está inválido")]
        public string Email { get; set; }

        [DisplayName("Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(30, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [DisplayName("Confirmar Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Compare(nameof(Password), ErrorMessage = "As senhas devem ser iguais")]
        public string ConfirmPassword { get; set; }
    }
}
