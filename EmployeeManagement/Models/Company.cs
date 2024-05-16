using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Company
    {
        [Key]
        [Required(ErrorMessage ="Company Field is Required.")]
        public String Company_Name {  get; set; }

        [Required(ErrorMessage ="This Field is Required.")]
        public String Contact_person { get; set;}

        [Required(ErrorMessage ="Email Field is Required.")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(20)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter a correct email")]
        public String Email {  get; set;}

        [Required(ErrorMessage = "Mobile Field is Required.")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{10}$")]
        public String Phone { get; set;}

        [Required(ErrorMessage = "Address Field Reqiured..")]
        public String Address {  get; set;}

        [Required(ErrorMessage ="Country Field is Required.")]
        public String Country {  get; set;}

    }
}
