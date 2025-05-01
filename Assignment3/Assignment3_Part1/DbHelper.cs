using MySql.Data.MySqlClient;

namespace Assignment3_Part1
{
    public class DbHelper
    {
        private static string connectionString = "server=localhost;user=Dev;password=Dev123;database=e_sports;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
