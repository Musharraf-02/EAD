using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_Blogs.Models
{
    public class User
    {
        [Required(ErrorMessage = "Enter username")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string username { get; set; }
        [Required(ErrorMessage = "Enter email")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required(ErrorMessage = "Enter password")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string photo { get; set; }
        public int userId { get; set; }
        public void addUser()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"insert into [user](username,email,password,photo) values (@username,@email,@password,@default)";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("username", this.username);
            SqlParameter sqlParameter2 = new SqlParameter("email", this.email);
            SqlParameter sqlParameter3 = new SqlParameter("password", this.password);
            SqlParameter sqlParameter4 = new SqlParameter("default", "default");
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlCommand.Parameters.Add(sqlParameter2);
            sqlCommand.Parameters.Add(sqlParameter3);
            sqlCommand.Parameters.Add(sqlParameter4);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public bool authenticateUser()
        {
            if (this.username != null && this.password != null)
            {
                string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
                SqlConnection sqlConnection = new SqlConnection(conn);
                string query = $"select * from [user] where username=@username and password=@password COLLATE Latin1_General_CS_AS ";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlParameter sqlParameter1 = new SqlParameter("username", this.username);
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
        public bool checkDuplicateUsername()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"select username from [user] where username=@username";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("username", this.username);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                return true;
            }
            sqlConnection.Close();
            return false;
        }

        public int getUserId()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"select Id from [user] where username=@username and password=@password COLLATE Latin1_General_CS_AS ";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("username", this.username);
            SqlParameter sqlParameter3 = new SqlParameter("password", this.password);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlCommand.Parameters.Add(sqlParameter3);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                return int.Parse(sqlDataReader[0].ToString());
            }
            sqlConnection.Close();
            return 0;
        }
        public string getUserName()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"select username from [user] where Id=@id";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("id", this.userId);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                return sqlDataReader[0].ToString();
            }
            sqlConnection.Close();
            return "";
        }
        public string getUserPhoto()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"select photo from [user] where Id=@id";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("id", this.userId);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                return sqlDataReader[0].ToString();
            }
            sqlConnection.Close();
            return "";
        }
        public string getUserEmail()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"select email from [user] where Id=@id";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("id", this.userId);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            User user = new User();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                return sqlDataReader[0].ToString();
            }
            sqlConnection.Close();
            return "";
        }
        public void updateUser(IFormFile photo)
        {
            User userCopy = this;
            userCopy.photo = userCopy.getUserPhoto();
            string guid = Guid.NewGuid().ToString();
            if (photo != null)
            {
                string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", guid + ".jpg");
                photo.CopyTo(new FileStream(photoPath, FileMode.Create));
            }
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"update [user] set username=@username, email=@email, password=@password,photo=@photo where id=@userID";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("username", this.username);
            SqlParameter sqlParameter2 = new SqlParameter("email", this.email);
            SqlParameter sqlParameter4 = new SqlParameter("password", this.password);
            SqlParameter sqlParameter3 = new SqlParameter("userID", this.userId);
            SqlParameter sqlParameter5 = new SqlParameter();
            if (photo != null)
            {
                sqlParameter5 = new SqlParameter("photo", guid.ToString());
            }
            else
            {
                sqlParameter5 = new SqlParameter("photo", userCopy.photo);
            }
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlCommand.Parameters.Add(sqlParameter2);
            sqlCommand.Parameters.Add(sqlParameter3);
            sqlCommand.Parameters.Add(sqlParameter4);
            sqlCommand.Parameters.Add(sqlParameter5);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
        public List<User> getAllUsers()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"Select * from [user]";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<User> userList = new List<User>();
            while (sqlDataReader.Read())
            {
                User user = new User();
                user.userId = int.Parse(sqlDataReader[0].ToString());
                user.username = sqlDataReader[1].ToString();
                user.email = sqlDataReader[2].ToString();
                user.photo = sqlDataReader[4].ToString();
                userList.Add(user);
            }
            sqlConnection.Close();
            return userList;
        }
        public void deleteUser()
        {
            Blog blog = new Blog();
            blog.userID = this.userId;
            blog.deleteBlogWithUserId();
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"delete from [user] where id=@userID";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("userID", this.userId);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public User getUser()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"Select * from [user] where id=@id";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("id", this.userId);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            User user = new User();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                user.userId = int.Parse(sqlDataReader[0].ToString());
                user.username = (sqlDataReader[1].ToString());
                user.email = (sqlDataReader[2].ToString());
                user.password = (sqlDataReader[3].ToString());
                user.photo = (sqlDataReader[4].ToString());
            }
            sqlConnection.Close();
            return user;
        }
    }
}