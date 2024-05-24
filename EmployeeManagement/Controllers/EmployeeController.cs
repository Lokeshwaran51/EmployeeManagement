using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDAL _employeeDAL;  // Used to Access the Data from EmployeeDAL Class

        public EmployeeController(EmployeeDAL employeeDAL)
        {
            _employeeDAL = employeeDAL;
        }

        //It returns empty form 
        [HttpGet]
        public IActionResult AddEmployee()
        {
            List<Company> companyList = _employeeDAL.GetCompanies();
            ViewBag.companyList1 = new SelectList(companyList, "Company_Id", "Company_Name");

            return View();
        }


        [HttpPost]
        public IActionResult InsertEmployeeDetails(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var emailExists = _employeeDAL.CheckEmployeeEmail(employee.Email);
                if (emailExists)
                {
                    TempData["EmailExist"] = "Registered email already exists.";
                    return RedirectToAction("AddEmployee", "Employee"); // Return the view with the same model to display the error message
                }

                var isInserted = _employeeDAL.AddEmployee(employee);
                if (isInserted)
                {
                    TempData["AddMessage"] = "Employee added successfully.";
                    return RedirectToAction("ListEmployees", "Employee"); // Redirect to the list of employees
                }
            }
            return View(employee);
        }



        //UpdateEmployeeById  GetMethod
        [HttpGet]
        public IActionResult UpdateEmployee(int Emp_Id)
        {
            var emp=_employeeDAL.GetEmployeeById(Emp_Id); //It gets particular employee details from db using emp_id

            List<Company> companyList = _employeeDAL.GetCompanies();  //Here i want to show DropDown list in Update form
            ViewBag.companyList1 = new SelectList(companyList, "Company_Id", "Company_Name");
            return View(emp);
        }


        // Insert the Updated Values to db post method
        [HttpPost]
        public IActionResult UpdateEmployeeById(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var updateEmp = _employeeDAL.UpdateEmp(employee);
                if ( _employeeDAL.UpdateEmp(employee))
                {
                    TempData["UpdateMessage"] = "Employee updated successfully.";
                    return RedirectToAction("ListEmployees");
                }
            }
            return View(employee);
        }

        //DeleteEmpById before soft deletion is happen
        public IActionResult SoftDeleteById(int Emp_Id)
        {
            var deleteEmp = _employeeDAL.DeleteEmployeeById(Emp_Id);
            TempData["DeleteMessage"] = "Employee record deleted successfully.";
            return RedirectToAction("ListEmployees");
        }


        //List Employee Details in table after the Soft Deletion 
        [HttpGet]
        public IActionResult ListEmployees()
        {
            var activeEmployees = _employeeDAL.GetAllEmployees();
            return View(activeEmployees);
        }
    }
}
