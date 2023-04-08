using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TwoFA : BaseEntity
    {
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public string key { get; set; }
        public DateTime ExpirtionDate { get; set; }
    }
}
