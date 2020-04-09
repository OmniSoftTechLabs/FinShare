using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace DAL
{
    public class LeadGenerator
    {
        public Guid? LeadGeneratorID { get; set; }

        [DisplayName("Name")] 
        [Required]
        public string LeadGeneratorName { get; set; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid contact number")]
        [DisplayName("Contact No")]
        public string ContactNo1 { get; set; }
        
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid contact number")]  
        [DisplayName("Contact No")]
        public string ContactNo2 { get; set; }

        [Display(Name = "Email ID")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailID { get; set; }
    
        //[Required]
        //public Decimal? Share { get; set; }
        public DateTime? EntDate { get; set; }
        public Guid? EntBy { get; set; }
        public DateTime? ModDate { get; set; }
        public Guid? ModBy { get; set; }
    }
}
