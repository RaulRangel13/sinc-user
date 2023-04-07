using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class RecoverPasswordModel
    {
        public string Email { get; set; }
        public string BaseUrl { get; set; }
    }
}
