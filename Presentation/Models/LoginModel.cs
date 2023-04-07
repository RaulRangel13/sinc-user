using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está inválido")]
        public string Email { get; set; }

        [DisplayName("Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Password { get; set; }

    }
}
