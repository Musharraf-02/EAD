using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_Blogs.Models
{
    public class Blog
    {
        public int blogID { get; set; }
        [Required(ErrorMessage = "Enter title")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string title { get; set; }
        [Required(ErrorMessage = "Enter description")]
        [DataType(DataType.Text)]
        public string deccription { get; set; }
        public DateTime date { get; set; }
        public string username { get; set; }
        public int userID { get; set; }
        public string photo { get; set; }
        public static List<Blog> getAllBlogs()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"Select * from blog order by date";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<Blog> blogsList = new List<Blog>();
            while (sqlDataReader.Read())
            {
                Blog blog = new Blog();
                blog.blogID = int.Parse(sqlDataReader[0].ToString());
                blog.title = sqlDataReader[1].ToString();
                blog.deccription = sqlDataReader[2].ToString();
                blog.date = DateTime.Parse(sqlDataReader[3].ToString());
                blog.userID = int.Parse(sqlDataReader[4].ToString());
                blogsList.Add(blog);
            }
            sqlConnection.Close();
            query = $"Select u.username, u.Id, u.photo from [user] as U , Blog as B where U.id=B.userID";
            sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            sqlDataReader = sqlCommand.ExecuteReader();
            int i = 0;
            List<string> usernames = new List<string>();
            List<string> photos = new List<string>();
            List<int> uIDs = new List<int>();
            while (sqlDataReader.Read())
            {
                usernames.Add(sqlDataReader[0].ToString());
                photos.Add(sqlDataReader[2].ToString());
                uIDs.Add(int.Parse(sqlDataReader[1].ToString()));
                i++;
            }
            sqlConnection.Close();
            for (int j = 0; j < blogsList.Count; j++)
            {
                for (int k = 0; k < uIDs.Count; k++)
                {
                    if (blogsList[j].userID == uIDs[k])
                    {
                        blogsList[j].username = usernames[k];
                        blogsList[j].photo = photos[k];
                    }
                }
            }
            return blogsList;
        }
        public void addBlog()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"insert into blog(title,description,date,userID) values (@title,@desc,@date,@userID)";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("title", this.title);
            SqlParameter sqlParameter2 = new SqlParameter("desc", this.deccription);
            SqlParameter sqlParameter3 = new SqlParameter("date", this.date);
            SqlParameter sqlParameter4 = new SqlParameter("userID", this.userID);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlCommand.Parameters.Add(sqlParameter2);
            sqlCommand.Parameters.Add(sqlParameter3);
            sqlCommand.Parameters.Add(sqlParameter4);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public Blog getBLog()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"Select * from blog where Id=@blogID";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("blogID", this.blogID);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            Blog blog = new Blog();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                blog.blogID = this.blogID;
                blog.title = sqlDataReader[1].ToString();
                blog.deccription = sqlDataReader[2].ToString();
                blog.date = DateTime.Parse(sqlDataReader[3].ToString());
                blog.userID = int.Parse(sqlDataReader[4].ToString());
                User user = new User();
                user.userId = blog.userID;
                blog.username = user.getUserName();
            }
            sqlConnection.Close();
            return blog;
        }
        public void deleteBlog()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"delete from blog where id=@blogID";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("blogID", this.blogID);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public void deleteBlogWithUserId()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"delete from blog where userid=@userID";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("userID", this.userID);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public void updateBlog()
        {
            string conn = @"Data Source = SQL5097.site4now.net; Initial Catalog = db_a75556_techblogs; User Id = db_a75556_techblogs_admin; Password = FreeHosting4102";
            SqlConnection sqlConnection = new SqlConnection(conn);
            string query = $"update blog set title=@title, description=@desc, date=@date where id=@blogId";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlParameter sqlParameter1 = new SqlParameter("title", this.title);
            SqlParameter sqlParameter2 = new SqlParameter("desc", this.deccription);
            SqlParameter sqlParameter4 = new SqlParameter("date", this.date);
            SqlParameter sqlParameter3 = new SqlParameter("blogId", this.blogID);
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlCommand.Parameters.Add(sqlParameter2);
            sqlCommand.Parameters.Add(sqlParameter3);
            sqlCommand.Parameters.Add(sqlParameter4);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}