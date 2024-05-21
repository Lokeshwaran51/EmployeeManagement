using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using EmployeeManagement.Models;

namespace EmployeeManagement.DataAccess
{
    public class EmployeeDAL
    {
        private readonly IConfiguration _configuration;  //Read the Connection from Connection String
        public EmployeeDAL(IConfiguration configuration)  
        {
            _configuration = configuration;
        }

        //Insert
        public bool AddEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertEmployee", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_Name", employee.Company_Name);
                        cmd.Parameters.AddWithValue("@Name", employee.Name);
                        cmd.Parameters.AddWithValue("@Email", employee.Email);
                        cmd.Parameters.AddWithValue("@Mobile", employee.Mobile);
                        cmd.Parameters.AddWithValue("@Address", employee.Address);
                        cmd.Parameters.AddWithValue("@City", employee.City);
                        cmd.Parameters.AddWithValue("@PinCode", employee.PinCode);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //GetAll
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllEmployees", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee();

                                employee.Emp_Id = reader.GetInt32("Emp_Id");
                                employee.Company_Name = reader["Company_Name"].ToString();
                                employee.Name = reader["Name"].ToString();
                                employee.Email = reader["Email"].ToString();
                                employee.Mobile = reader["Mobile"].ToString();
                                employee.Address = reader["Address"].ToString();
                                employee.City = reader["City"].ToString();
                                employee.PinCode = reader["PinCode"].ToString();
                               
                                employeeList.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return employeeList;
        }

        //Delete
        public bool DeleteEmployeeById(int Emp_Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteEmpById", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_Id", Emp_Id);
                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if(rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool UpdateEmp(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UpdateEmployeeDetailsById]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_Id", employee.Emp_Id);
                   // cmd.Parameters.AddWithValue("@Company_Id", employee.Company_Id);
                    cmd.Parameters.AddWithValue("@Company_Name", employee.Company_Name);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Mobile", employee.Mobile);
                    cmd.Parameters.AddWithValue("@Address", employee.Address);
                    cmd.Parameters.AddWithValue("@City", employee.City);
                    cmd.Parameters.AddWithValue("@PinCode", employee.PinCode);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        //Update
        public Employee GetEmpById(int Emp_Id)
        {
            Employee employee = new Employee();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[EmpGetById]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_Id", Emp_Id);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        employee.Company_Name = reader["Company_Name"].ToString();
                        employee.Name = reader["Name"].ToString();
                        employee.Email = reader["Email"].ToString();
                        employee.Mobile = reader["Mobile"].ToString();
                        employee.Address = reader["Address"].ToString();
                        employee.City = reader["City"].ToString();
                        employee.PinCode = reader["PinCode"].ToString();
                    }
                    return employee;
                }
            }
        }

        public List<Company> GetCompanies()
        {
            List<Company> companies = new List<Company>();
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("GetCompany", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Company company = new Company();
                            company.Company_Id = Convert.ToInt32(reader["Company_Id"]);
                            company.Company_Name = reader["Company_Name"].ToString();
                            companies.Add(company);
                        }
                    }
                }  
            return companies;
        }
    }
}
