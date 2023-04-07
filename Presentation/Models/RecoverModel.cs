using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Presentation.Models
{
    public class RecoverModel
    {
        public string Token { get; set; }

        [DisplayName("Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Password { get; set; }
    }
}
