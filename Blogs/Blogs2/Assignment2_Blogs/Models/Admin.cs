using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_Blogs.Models
{
    public class Admin
    {
        [Required(ErrorMessage = "Enter username")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string name { get; set; }
        [Required(ErrorMessage = "Enter password")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public bool authenticateAdmin()
        {
            if (this.name != null && this.password != null)
            {
                string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
                SqlConnection sqlConnection = new SqlConnection(conn);
                string query = $"select * from admin where adminname=@name and password=@password COLLATE Latin1_General_CS_AS ";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlParameter sqlParameter1 = new SqlParameter("name", this.name);
                SqlParameter sqlParameter3 = new SqlParameter("password", this.password);
                sqlCommand.Parameters.Add(sqlParameter1);
                sqlCommand.Parameters.Add(sqlParameter3);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    return true;
                }
                sqlConnection.Close();
            }
            return false;
        }
    }
}