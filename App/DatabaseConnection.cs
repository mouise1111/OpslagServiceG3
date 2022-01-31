using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class DatabaseConnection
    {
        //private string connectionString = "Data Source=localhost,11433;User ID=sa;Password=MyVerySecurePassword123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public void Bewaar(string xml)

        {
            string connectionString = "Data Source=localhost,11433;Initial Catalog=XMLDatabase;User ID=sa;Password=MyVerySecurePassword123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            string insertSqlText = "INSERT INTO XMLFiles (XML) VALUES(@xml)";

            SqlCommand insertSql = new SqlCommand(insertSqlText);

            insertSql.Connection = new SqlConnection(connectionString);

            insertSql.Parameters.Add(new SqlParameter("@xml", xml));



            try

            {

                insertSql.Connection.Open();
                insertSql.ExecuteNonQuery();

            }

            catch (SqlException ex)

            {

                Console.WriteLine("Bewaar Rss mislukt." + ex.Message);

            }

            finally

            {

                if (insertSql.Connection.State == ConnectionState.Open)

                {

                    insertSql.Connection.Close();

                }

            }

        }

    }
}
