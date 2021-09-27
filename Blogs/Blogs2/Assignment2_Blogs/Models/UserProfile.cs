using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_Blogs.Models
{
    public class UserProfile
    {
        [Required(ErrorMessage = "Enter username")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string username { get; set; }
        [Required(ErrorMessage = "Enter email")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
        public string photoName { get; set; }
        public IFormFile photo { get; set; }
    }
}