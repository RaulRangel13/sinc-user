using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Responses
{
    public class CustomerResponse
    {
        public string? Email { get; set; }
        public bool Sucess { get; private set; }
        public List<string>? ErrorsMessage { get; private set; }
        public CustomerResponse(bool sucess = true)
        {
            Sucess = sucess;
            if(!sucess)
                ErrorsMessage = new List<string>();
        }
        public void AddErrors(IEnumerable<string> erros) =>
            ErrorsMessage?.AddRange(erros);
    }
}
