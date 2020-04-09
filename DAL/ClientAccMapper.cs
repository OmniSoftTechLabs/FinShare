using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ClientAccMapper
    {
        public int Uid { get; set; }
        public Guid? ClientID { get; set; }

        [DisplayName("Name")]
        public string ClientName { get; set; }

        [DisplayName("Account Number")]
        [Required]
        public string AccountNumber { get; set; }
    }
}
