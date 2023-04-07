using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class RecoverEmailPassModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está inválido")]
        public string Email { get; set; }
    }
}
