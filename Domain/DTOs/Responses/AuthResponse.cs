using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Responses
{
    public sealed class AuthResponse
    {
        public string Token { get; set; }
        public string Type { get; set; }
        public int ExpireIn { get; set; }
    }
}
