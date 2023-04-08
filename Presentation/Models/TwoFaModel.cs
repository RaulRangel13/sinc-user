using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class TwoFaModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string? Key { get; set; }
    }
}
