using System.Data.SqlClient;

namespace LMS_Assignment5
{


    public static class DBConnection
    {
        private static string connectionString = @"Data Source=SANDEEP\SQLEXPRESS;Initial Catalog=AssignmentDB;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void TestConnection()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Database connection successful!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed: " + ex.Message);
                }
            }
        }
    }
}