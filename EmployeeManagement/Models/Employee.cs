using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        [Key]
        [Required]
        public int Emp_Id { get; set; }

        [Required(ErrorMessage ="CompanyName is Required..")]
        public String Company_Name { get; set; }

        [Required(ErrorMessage ="Name Field is Required.")]
        public String Name {  get; set; }

        [Required(ErrorMessage ="Email Field is Required.")]
        [EmailAddress(ErrorMessage ="Invalid Email Address..")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(30)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter a correct email")]
        public String Email {  get; set; }

        [Required(ErrorMessage ="Gender Field is Required..")]
        public String Gender {  get; set; }

       
        public String? AreaOfInterest { get; set; } 

        [Required(ErrorMessage ="Mobile Field is Required.")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{10}$")]
        public String Mobile {  get; set; }

        [Required(ErrorMessage ="Address Field Reqiured..")]
        public String Address {  get; set; }

        [Required(ErrorMessage ="City is Required.")]
        public String City { get; set; }

        [Required(ErrorMessage = "Pincode is required.")]
        [RegularExpression(@"^6[0-9]{5}$", ErrorMessage = "Pincode must be exactly 6 digits and start with 6.")]
        public string PinCode { get; set; }

        public List<String>? SelectedAreas {  get; set; }    
        
        public Employee()
        {
            SelectedAreas = new List<String>(); 
        }
    }
}
