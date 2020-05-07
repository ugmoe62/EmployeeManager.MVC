using System;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManager.MVC.Security
{
    public class AppIndentityUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
