using Assignment2_Blogs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_Blogs.Controllers
{
    public class BlogController : Controller
    {
         
        public ViewResult Index()
        {
            return View("login");
        }
        [HttpPost]
        public ViewResult Index(string username, string password)
        {
            User user = new User();
            user.username = username;
            user.password = password;
           
            if (user.authenticateUser())
            {
                
                HttpContext.Session.SetString("uid", user.getUserId().ToString());
                HttpContext.Session.SetString("passwd", password);
                HttpContext.Session.SetString("uname", username);
                return Home();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View("login");
            }
        }
        public ViewResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Signup(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.checkDuplicateUsername())
                {
                    ModelState.AddModelError(string.Empty, "Username already taken.");
                    return View();
                }
                else
                {
                    user.addUser();
                    return View("thanks");
                }
            }
            else
            {
                return View();
            }
        }
        public ViewResult newPost()
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                return View();
            }
            else
            {
                return View("login");
            }
        }
        [HttpPost]
        public ViewResult newPost(Blog blog)
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                if (ModelState.IsValid)
                {
                   
                    blog.userID = Convert.ToInt32( HttpContext.Session.GetString("uid"));
                    blog.date = DateTime.Now;
                    blog.addBlog();
                    return Home();
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult openBlog(int id)
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                Blog blog = new Blog();
                blog.blogID = id;
                blog = blog.getBLog();
                if (blog.userID ==Convert.ToInt32( HttpContext.Session.GetString("uid")))
                {
                    return View("openYourBlog", blog);
                }
                else
                {
                    return View("openBlog", blog);
                }
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult deleteBlog(int id)
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                Blog blog = new Blog();
                blog.blogID = id;
                blog.deleteBlog();
                return Home();
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult updateBlog(int id)
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                Blog blog = new Blog();
                blog.blogID = id;
                HttpContext.Session.SetString("bid", id.ToString());
                blog = blog.getBLog();
                return View("updateBlog", blog);
            }
            else
            {
                return View("login");
            }
        }
        [HttpPost]
        public ViewResult updateBlog(Blog blog)
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                if (ModelState.IsValid)
                {
                    blog.userID = Convert.ToInt32( HttpContext.Session.GetString("uid"));
                    blog.blogID = Convert.ToInt32( HttpContext.Session.GetString("bid"));
                    blog.date = DateTime.Now;
                    blog.updateBlog();
                    return Home();
                }
                else
                {
                    return View("updateBlog", blog);
                }
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult Home()
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                HomePageData homePageData = new HomePageData(Convert.ToInt32( HttpContext.Session.GetString("uid")));
                return View("home", homePageData);
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult About()
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                return View();
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult Profile()
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                User user = new User();
                user.userId = Convert.ToInt32( HttpContext.Session.GetString("uid"));
                user.password = HttpContext.Session.GetString("passwd"); ;
                user.username = HttpContext.Session.GetString("uname");
                UserProfile up = new UserProfile();
                up.username = HttpContext.Session.GetString("uname"); ;
                up.email = user.getUserEmail();
                up.photoName = user.getUserPhoto();
                return View("profile", up);
            }
            else
            {
                return View("login");
            }
        }
        [HttpPost]
        public ViewResult Profile(UserProfile up)
        {
            if (HttpContext.Session.Keys.Contains("passwd"))
            {
                User user1 = new User();
                user1.password = HttpContext.Session.GetString("passwd"); ;
                user1.username = HttpContext.Session.GetString("uname"); ;
                user1.userId = Convert.ToInt32(HttpContext.Session.GetString("uid"));
                if (ModelState.IsValid)
                {
                    if (up.newPassword != HttpContext.Session.GetString("passwd") && up.newPassword != null && up.password != null && up.password == HttpContext.Session.GetString("passwd"))
                    {
                        
                        HttpContext.Session.SetString("passwd", up.newPassword);
                       
                        HttpContext.Session.SetString("uname", up.username);
                        User user = new User();
                        user.username = HttpContext.Session.GetString("uname");
                        user.password = HttpContext.Session.GetString("passwd");
                        user.email = up.email;
                        user.userId = Convert.ToInt32(HttpContext.Session.GetString("uid"));
                        user.updateUser(up.photo);
                        return Home();
                    }
                    else if (up.newPassword == HttpContext.Session.GetString("passwd") && up.newPassword != null && up.password != null && up.password == HttpContext.Session.GetString("passwd"))
                    {
                        up.photoName = user1.getUserPhoto();
                        ModelState.AddModelError(string.Empty, "New password cannot be same as old password.");
                        return View("Profile", up);
                    }
                    else if (up.newPassword != HttpContext.Session.GetString("passwd") && up.newPassword != null && up.password != null && up.password != HttpContext.Session.GetString("passwd"))
                    {
                        up.photoName = user1.getUserPhoto();
                        ModelState.AddModelError(string.Empty, "Old password is incorrect.");
                        return View("Profile", up);
                    }
                    else if (up.password == null && up.newPassword != null)
                    {
                        up.photoName = user1.getUserPhoto();
                        ModelState.AddModelError(string.Empty, "Enter old password.");
                        return View("Profile", up);
                    }
                    else if (up.password != null && up.newPassword == null)
                    {
                        up.photoName = user1.getUserPhoto();
                        ModelState.AddModelError(string.Empty, "Enter new password.");
                        return View("Profile", up);
                    }
                    else
                    {
                      
                        HttpContext.Session.SetString("uname", up.username);
                        User user = new User();
                        user.username = HttpContext.Session.GetString("uname");
                        user.password = HttpContext.Session.GetString("passwd");
                        user.email = up.email;
                        user.userId = Convert.ToInt32(HttpContext.Session.GetString("uid"));
                        user.updateUser(up.photo);
                        return Home();
                    }
                }
                else
                {
                    up.photoName = user1.getUserPhoto();
                    return View("Profile", up);
                }
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult logout()
        {
            HttpContext.Session.Remove("uname");
            HttpContext.Session.Remove("passwd");
            HttpContext.Session.Remove("uid");
            HttpContext.Session.Remove("bid");
            return View("login");
        }
    }
}