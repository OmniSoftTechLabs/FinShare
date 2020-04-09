using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;



namespace DAL
{
    public class Client
    {


        public Guid? ClientID { get; set; }

        [DisplayName("Name")]
        [Required]
        public string ClientName { get; set; }
        [DisplayName("Client Alias Name")]

        public string ClientAlieas { get; set; }

        [Required]
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessage = "Please Enter Valid Postal Code.")]
        public decimal? PinCode { get; set; }

        [DisplayName("PAN No")]
        public string PANNo { get; set; }

        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        [DisplayName("Account No")]
        [Required]
        public string AccountNo { get; set; }

        public string IFSC { get; set; }

        public string MICR { get; set; }

        [Display(Name = "Email ID")]
        //// [Required(ErrorMessage = "The email address is required")]
        // [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailID { get; set; }

        [DisplayName("Contact No")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid contact number")]
        public string ContactNo1 { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid contact number")]
        [DisplayName("Contact No")]
        public string ContactNo2 { get; set; }

        [Required]
        [DisplayName("Lead Generator 1")]
        public Guid? LG1 { get; set; }

        [DisplayName("Lead Generator 2")]
        public Guid? LG2 { get; set; }

        [Required]
        [DisplayName("LG1 Share")]
        [Range(0, 100)]
        public Decimal? LG1Share { get; set; }

        [DisplayName("LG2 Share")]
        [Range(0, 100)]
        public Decimal? LG2Share { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        public string Nominee { get; set; }

        public DateTime? EntDate { get; set; }
        public Guid EntBy { get; set; }
        public DateTime? ModDate { get; set; }
        public Guid? ModBy { get; set; }


        public List<LeadGenerator> LGlist { get; set; }
        public List<ClientAccMapper> AccountNumberList { get; set; }
        //  public System.Collections.IEnumerable LGList { get; set; }
        //public System.Collections.IEnumerable LGList;
    }
}
