using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_Blogs.Models
{
    public class HomePageData
    {
        public List<Blog> blogsList { get; set; }
        public int id { get; set; }
        public HomePageData(int id)
        {
            blogsList = Blog.getAllBlogs();
            this.id = id;
        }
    }
}