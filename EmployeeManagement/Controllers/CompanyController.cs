using EmployeeManagement.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyDAL _companyDAL;

        public CompanyController(CompanyDAL companyDAL)
        {
            _companyDAL = companyDAL;
        }

        [HttpGet]
        public IActionResult Company()
        {
            List<Country> countryList = _companyDAL.GetCountry();
            ViewBag.countryList = new SelectList(countryList, "Country_Id", "Country_Name");
            return View();
        }

        [HttpPost]
        public IActionResult Company(Company _company)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _companyDAL.AddCompany(_company);
               if (isInserted)
                {
                    TempData["InsertCompany"] = "Company details added successfully..";
                }
                else
                {
                    TempData["InsertCompanyExists"] = "Company name already exists...";
                }
                return RedirectToAction("Company");
            }
            return View();
        }
    }
}
