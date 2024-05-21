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
                bool isInserted = _employeeDAL.AddEmployee(employee);
                if (isInserted)
                {
                    TempData["AddMessage"] = "Employee Added Successfully";
                    return RedirectToAction("ListEmployees");
                }
                else
                {
                    return RedirectToAction("AddEmployee");
                }
            }
            return RedirectToAction("AddEmployee");
        }

        [HttpGet]
        public IActionResult ListEmployees()
        {
                var employees = _employeeDAL.GetAllEmployees();
                return View(employees);
        }

        public IActionResult DeleteEmpById(int Emp_Id)
        {
               var deleteEmp= _employeeDAL.DeleteEmployeeById(Emp_Id);
            TempData["DeleteMessage"] = "Employee record deleted successfully.";
            return RedirectToAction("ListEmployees");
        }

        [HttpGet]
        public IActionResult UpdateEmployee(int Emp_Id)
        {
            var emp=_employeeDAL.GetEmpById(Emp_Id);
            List<Company> companyList = _employeeDAL.GetCompanies();
            ViewBag.companyList1 = new SelectList(companyList, "Company_Id", "Company_Name");
            return View(emp);
        }


        [HttpPost]
        public IActionResult UpdateEmployeeById(Employee employee)
        {
            if (ModelState.IsValid)
            {
                bool updateEmp = _employeeDAL.UpdateEmp(employee);
                if (updateEmp)
                {
                    TempData["UpdateMessage"] = "Employee updated successfully.";
                    return RedirectToAction("ListEmployees");
                }
                else
                {
                    TempData["UpdateMessage"] = "Failed to update employee.";
                    return View(employee);
                }
            }
            return View(employee);
        }
    }
}
