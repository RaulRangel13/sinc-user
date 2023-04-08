using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class TwoFaValidateModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Key { get; set; }
    }
}
