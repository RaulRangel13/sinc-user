using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests
{
    public class TwoFaRequestDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Key { get; set; }
    }
}
