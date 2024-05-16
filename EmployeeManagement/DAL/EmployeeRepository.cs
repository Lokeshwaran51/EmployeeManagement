using EmployeeManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagement.DAL
{
    public class EmployeeRepository
    {
        private SqlConnection connection;
        private EmployeeRepository()
        {
            String ConnectionStr = "DefaultConnection";
            connection = new SqlConnection(ConnectionStr);
        }
        public void InsertUser(Employee employee)
        {
            SqlCommand insertCommand = new SqlCommand("InsertUser", connection);
            insertCommand.CommandType=CommandType.StoredProcedure;
            insertCommand.Parameters.AddWithValue("@Company_Name", employee.Company_Name);
            insertCommand.Parameters.AddWithValue("@Name", employee.Name);
            insertCommand.Parameters.AddWithValue("@Email", employee.Email);
            insertCommand.Parameters.AddWithValue("@Mobile", employee.Mobile);
            insertCommand.Parameters.AddWithValue("@Address", employee.Address);
            insertCommand.Parameters.AddWithValue("@City", employee.City);
            insertCommand.Parameters.AddWithValue("@PinCode", employee.PinCode);

            connection.Open();
            int rowsAffected = insertCommand.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("User Inserted Successfully");
            }
            else
            {
                Console.WriteLine("User Not Inserted");
            }
        }
    }
}
