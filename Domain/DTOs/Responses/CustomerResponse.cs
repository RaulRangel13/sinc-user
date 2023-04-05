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
        public bool Sucesso { get; private set; }
        public List<string>? ErrosMessage { get; private set; }
        public CustomerResponse(bool sucesso = true)
        {
            Sucesso = sucesso;
        }
        public void AdicionarErros(IEnumerable<string> erros) =>
            ErrosMessage?.AddRange(erros);
    }
}
