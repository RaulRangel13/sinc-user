using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Settings
{
    public class AuthSettings
    {
        public string SecretKey { get; set; }
        public string ExpireIn { get; set; }
    }
}
