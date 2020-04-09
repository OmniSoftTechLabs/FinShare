using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
namespace DAL
{
    public class ImportExcel
    {
        public Guid ImpID { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [DisplayName("AMC")]
        public string AMCID { get; set; }
        public string AMCCode { get; set; }

        public DateTime? EntDate { get; set; }
        public Guid EntBy { get; set; }
        public DateTime? ModDate { get; set; }
        public string ModBy { get; set; }
        public List<impyears> years { get; set; }
        public List<AMC> AMCS { get; set; }
        public string File { get; set; }
        public string SuccessMsg { get; set; }
    }
    public class impyears
    {
        public int Id { get; set; }
        public int Name { get; set; }
    }

    public class AMC
    {
        public string AMCID { get; set; }
        public string AMCCode { get; set; }
    }
}
