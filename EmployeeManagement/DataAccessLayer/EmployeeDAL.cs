using Microsoft.Data.SqlClient;
using System.Data;
using EmployeeManagement.Models;

namespace EmployeeManagement.DataAccess
{
    public class EmployeeDAL
    {
        private readonly IConfiguration _configuration;

        public EmployeeDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        // Insert
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
                        cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                        cmd.Parameters.AddWithValue("@Mobile", employee.Mobile);
                        cmd.Parameters.AddWithValue("@Address", employee.Address);
                        cmd.Parameters.AddWithValue("@City", employee.City);
                        cmd.Parameters.AddWithValue("@PinCode", employee.PinCode);

                        // Create a DataTable for the AreaOfInterest parameter
                        DataTable areaOfInterestTable = new DataTable();
                        areaOfInterestTable.Columns.Add("AreaOfInterest_Id1", typeof(int));
                        foreach (var area in employee.SelectedAreas)
                        { 
                            areaOfInterestTable.Rows.Add(area);
                        }
                        // Create a SqlParameter for the table-valued parameter
                        SqlParameter areaOfInterestParam = new SqlParameter("@AreaOfInterest", SqlDbType.Structured);
                        
                        areaOfInterestParam.TypeName = "dbo.AreaTableTypes";
                        areaOfInterestParam.Value = areaOfInterestTable;
                        
                        cmd.Parameters.Add(areaOfInterestParam);

                       connection.Open();
                        cmd.ExecuteNonQuery();
                        return true; 
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }



        // Get All Employees
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[CheckEmployeeIsActive]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee
                                {
                                    Emp_Id = reader.GetInt32(reader.GetOrdinal("Emp_Id")),
                                    Company_Name = reader["Company_Name"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    AreaOfInterest = reader["AreaOfInterest"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    City = reader["City"].ToString(),
                                    PinCode = reader["PinCode"].ToString()
                                };

                                employeeList.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return employeeList;
        }

        // Soft Delete
        public bool DeleteEmployeeById(int empId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("SoftDelete_Get", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Emp_Id", empId);
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

        // Update Employee Details
        public bool UpdateEmp(Employee employee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateEmployeeDetailsById", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Emp_Id", employee.Emp_Id);
                        cmd.Parameters.AddWithValue("@Company_Name", employee.Company_Name);
                        cmd.Parameters.AddWithValue("@Name", employee.Name);
                        cmd.Parameters.AddWithValue("@Email", employee.Email);
                        cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                     
                        DataTable areaOfInterestTable = new DataTable();
                        areaOfInterestTable.Columns.Add("AreaOfInterest_Id1", typeof(int));
                        employee.SelectedAreas.ForEach(area => areaOfInterestTable.Rows.Add(area));

                        // Create a SqlParameter for the table-valued parameter
                        SqlParameter areaOf = new SqlParameter("@AreaOfInterest", SqlDbType.Structured);

                        areaOf.TypeName = "dbo.AreaTable";
                        areaOf.Value = areaOfInterestTable;
                        cmd.Parameters.Add(areaOf);

                        cmd.Parameters.AddWithValue("@Mobile", employee.Mobile);
                        cmd.Parameters.AddWithValue("@Address", employee.Address);
                        cmd.Parameters.AddWithValue("@City", employee.City);
                        cmd.Parameters.AddWithValue("@PinCode", employee.PinCode);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        // Get Employee By Id
        public Employee GetEmployeeById(int empId)
        {
            Employee employee = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("EmpGetById", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Emp_Id", empId);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employee = new Employee
                                {
                                    Emp_Id = reader.GetInt32(reader.GetOrdinal("Emp_Id")),
                                    Company_Name = reader["Company_Name"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    AreaOfInterest = reader["AreaOfInterest"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    City = reader["City"].ToString(),
                                    PinCode = reader["PinCode"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return employee;
        }

        // Get Companies from Company table
        public List<Company> GetCompanies()
        {
            List<Company> companies = new List<Company>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("GetCompany", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Company company = new Company
                                {
                                    Company_Id = Convert.ToInt32(reader["Company_Id"]),
                                    Company_Name = reader["Company_Name"].ToString()
                                };

                                companies.Add(company);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return companies;
        }

        // Check if Employee Email exists
        public bool CheckEmployeeEmail(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("EmailExists", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public List<AreaOfInt> GetArea()
        {
            List<AreaOfInt> areaOfInts = new List<AreaOfInt>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("Emp_Skillset", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AreaOfInt areaOfInt = new AreaOfInt();
                            areaOfInt.AreaOfInterest_Id1 = Convert.ToInt32(reader["AreaOfInterest_Id1"]);
                            areaOfInt.AreaOfInterest = reader["AreaOfInterest"].ToString();
                            areaOfInts.Add(areaOfInt);
                        }
                    }
                }
            }
            return areaOfInts;
        }
    }
}