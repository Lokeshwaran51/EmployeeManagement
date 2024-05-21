using Microsoft.Data.SqlClient;
using EmployeeManagement.Models;
using System.Data;

namespace EmployeeManagement.DataAccessLayer
{
    public class CompanyDAL
    {
        private readonly IConfiguration configuration;
        public CompanyDAL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool AddCompany(Company company)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("Insert_Company", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    
                        cmd.Parameters.AddWithValue("@Company_Name", company.Company_Name);
                        cmd.Parameters.AddWithValue("@Contact_person", company.Contact_person);
                        cmd.Parameters.AddWithValue("@Email", company.Email);
                        cmd.Parameters.AddWithValue("@Phone", company.Phone);
                        cmd.Parameters.AddWithValue("@Address", company.Address);
                        cmd.Parameters.AddWithValue("@Country", company.Country);
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {               
                return false;
            }
        }

        public List<Country> GetCountry()
        {
            List<Country> countries = new List<Country>();
            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("GetCountry", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Country country = new Country();
                            country.Country_Id = reader.GetInt32("Country_Id");
                            country.Country_Name = reader["Country_Name"].ToString();
                            countries.Add(country);
                        }
                    }
                }
            }
            return countries;
        }

        /*public bool InsertCompany(Company company)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("Insert_Company", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_Name", company.Company_Name);
                        cmd.Parameters.AddWithValue("@Contact_person", company.Contact_person);
                        cmd.Parameters.AddWithValue("@Email", company.Email);
                        cmd.Parameters.AddWithValue("@Phone", company.Phone);
                        cmd.Parameters.AddWithValue("@Address", company.Address);
                        cmd.Parameters.AddWithValue("@Country", company.Country);
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }*/

    }
}
