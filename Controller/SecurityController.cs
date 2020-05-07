using EmployeeManager.MVC.Models;
using EmployeeManager.MVC.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;





    public class SecurityController : Controller
    {
        private readonly UserManager<AppIndentityUser> userManager;
        private readonly RoleManager<AppIdentityRole> roleManager;
        private readonly SignInManager<AppIndentityUser> signInManager;

        public SecurityController(UserManager<AppIndentityUser> userManager,
        RoleManager<AppIdentityRole> roleManager,
        SignInManager<AppIndentityUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register obj)
        {
            if (ModelState.IsValid)
            {
                if (!roleManager.RoleExistsAsync("Manager").Result)
                {
                    AppIdentityRole role = new AppIdentityRole();
                    role.Name = "Manager";
                    role.Description = "Can perform CRUD operations.";
                    IdentityResult roleResult =
                        roleManager.CreateAsync(role).Result;
                }

                AppIndentityUser user = new AppIndentityUser();
                user.UserName = obj.UserName;
                user.Email = obj.Email;
                user.FullName = obj.FullName;
                user.BirthDate = obj.BirthDate;

                IdentityResult result = userManager.CreateAsync
                    (user, obj.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                return RedirectToAction("SignIn", "Security");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details!");
                }
            }
            return View(obj);
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignIn(SignIn obj)
        {
            if (ModelState.IsValid)
            {
                var result = signInManager.PasswordSignInAsync
                (obj.UserName, obj.Password, obj.RememberMe, false).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("List", "EmployeeManager");
                }
                else
                {
                    ModelState.AddModelError("", " Invalid user details!");
                }
            }
            return View(obj);
        }
            
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignOut()
        {
            signInManager.SignOutAsync().Wait();
        return RedirectToAction("SignIn", "Security");
    }
    

    }
    

