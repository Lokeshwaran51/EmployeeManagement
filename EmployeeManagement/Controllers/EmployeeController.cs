using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EmployeeManagement.Models;
using EmployeeManagement.DataAccess;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;    // Used to Read the Connection Details from ConnectionString 
        private readonly EmployeeDAL _employeeDAL;  // Used to Access the Data from EmployeeDAL Class

        public EmployeeController(IConfiguration configuration, EmployeeDAL employeeDAL)
        {
            _configuration = configuration;
            _employeeDAL = employeeDAL;
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertEmployeeDetails(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isInserted = _employeeDAL.InsertEmployee(employee);
                    if (isInserted)
                    {
                        return RedirectToAction("ListEmployees");
                    }
                    else
                    {
                        return RedirectToAction("AddEmployee");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return RedirectToAction("AddEmployee");
                }
            }
            else
            {
                return RedirectToAction("AddEmployee");
            }
        }

        [HttpGet]
        public IActionResult ListEmployees()
        {
            try
            {
                var employees = _employeeDAL.GetAllEmployees();
                return View(employees);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return View();
            }
        }

        //[HttpDelete]
        public IActionResult DeleteEmpById(int Emp_Id)
        {
            var deleteEmp = _employeeDAL.DeleteEmployeeById(Emp_Id);
                return RedirectToAction("ListEmployees"); 
        }

        [HttpPut]
        public IActionResult UpdateEmployeeById(int Emp_Id)
        {
            var updateEmp=_employeeDAL.UpdateEmpById(Emp_Id);
                return RedirectToAction("AddEmployee","Employee");
        }
    }
}