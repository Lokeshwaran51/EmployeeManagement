using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDAL _employeeDAL;

        public EmployeeController(EmployeeDAL employeeDAL)
        {
            _employeeDAL = employeeDAL;
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            try
            {
                List<Company> companyList = _employeeDAL.GetCompanies();
                ViewBag.companyList1 = new SelectList(companyList, "Company_Id", "Company_Name");

                List<AreaOfInt> AreaOfInterestList = _employeeDAL.GetArea();
                ViewBag.AreaOfInterestList1 = AreaOfInterestList;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error");
            }
        }


        [HttpPost]
        public IActionResult InsertEmployeeDetails(Employee employee, string[] AreaOfInterest)
        {
            if (ModelState.IsValid)
            {
                employee.AreaOfInterest = string.Join(",", AreaOfInterest);

                var emailExists = _employeeDAL.CheckEmployeeEmail(employee.Email);
                if (emailExists)
                {
                    TempData["EmailExist"] = "Registered email already exists.";
                    return RedirectToAction("AddEmployee", "Employee");
                }

                var isInserted = _employeeDAL.AddEmployee(employee);
                if (isInserted)
                {
                    TempData["AddMessage"] = "Employee added successfully.";
                    return RedirectToAction("ListEmployees", "Employee");
                }
            }
            return View("AddEmployee", employee);
        }

        [HttpGet]
        public IActionResult UpdateEmployee(int Emp_Id)
        {
            var emp = _employeeDAL.GetEmployeeById(Emp_Id);
            List<Company> companyList = _employeeDAL.GetCompanies();
            ViewBag.companyList1 = new SelectList(companyList, "Company_Id", "Company_Name");

            List<AreaOfInt> AreaOfInterestList = _employeeDAL.GetArea();
            ViewBag.AreaOfInterestList1 = AreaOfInterestList;
            return View(emp);
        }

        [HttpPost]
        public IActionResult UpdateEmployeeById(Employee employee, string[] AreaOfInterest)
        {
            if (ModelState.IsValid)
            {
                employee.AreaOfInterest = string.Join(",", AreaOfInterest);
                var updateEmp = _employeeDAL.UpdateEmp(employee);
                if (updateEmp)
                {
                    TempData["UpdateMessage"] = "Employee updated successfully.";
                    return RedirectToAction("ListEmployees");
                }
            }
            return View(employee);
        }

        public IActionResult SoftDeleteById(int Emp_Id)
        {
            var deleteEmp = _employeeDAL.DeleteEmployeeById(Emp_Id);
            TempData["DeleteMessage"] = "Employee record deleted successfully.";
            return RedirectToAction("ListEmployees");
        }

        public IActionResult ListEmployees()
        {
            var empList = _employeeDAL.GetAllEmployees();
            return View(empList);
        }
    }
}
