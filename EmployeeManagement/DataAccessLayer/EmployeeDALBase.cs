using EmployeeManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagement.DataAccess
{
    public class EmployeeDAL
    {

        public bool InsertUser(Employee employee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString("DefaultConnection")))
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
            }
        }
    }
}