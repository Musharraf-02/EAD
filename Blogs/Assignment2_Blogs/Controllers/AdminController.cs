using Assignment2_Blogs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_Blogs.Controllers
{
    public class AdminController : Controller
    {
        static string uname, passwd;
        public ViewResult index()
        {
            return View("login");
        }
        [HttpPost]
        public ViewResult index(Admin admin)
        {
            if (ModelState.IsValid)
            {
                if (admin.authenticateAdmin())
                {
                    uname = admin.name;
                    passwd = admin.password;
                    HttpContext.Session.SetString("admin", passwd);
                    return Home();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View("login");
                }
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult Home()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                User user = new User();
                List<User> userList = user.getAllUsers();
                return View("Home", userList);
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult deleteUser(int id)
        {
            if (HttpContext.Session.Keys.Contains("admin")) {
                User user = new User();
                user.userId = id;
                user.deleteUser();
                return Home();
            
            }
            else
            {
                return View("login");
            }
        }
        public ViewResult addUser()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                return View();
            }
            else
            {
                return View("login");
            }
        }
        [HttpPost]
        public ViewResult addUser(User user)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
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
                        return Home();
                    }
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
        public ViewResult updateUser(int id)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {

                User user = new User();
                user.userId = id;
                user = user.getUser();
                return View("updateUser", user);
            }
            else
            {
                return View("login");
            }
        }
        [HttpPost]
        public ViewResult updateUser(User user)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                User user1 = new User();
                user1.userId = user.userId;
                user1 = user1.getUser();
                user.password = user1.password;
                if (user.username != null && user.email != null)
                {
                    IFormFile photo = null;
                    user.updateUser(photo);
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
        public ViewResult Logout()
        {
            HttpContext.Session.Remove("admin");
            uname = null;
            passwd = null;
            return View("login");
        }
    }
}