using BniSittingManager.Data;
using BniSittingManager.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Security.Claims;

namespace BniSittingManager.Controllers
{
    public class AccountController : Controller
    {

        private readonly IDbLayer _dbLayer;

        public AccountController(IDbLayer dbLayer)
        {
            _dbLayer = dbLayer;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            SqlParameter[] parameters =
            {
        new SqlParameter("@Action", "LOGIN"),
        new SqlParameter("@UserName", model.Username),
        new SqlParameter("@Password", model.Password)
    };

            DataTable dt = await _dbLayer.ExecuteSPAsync("sp_UserAccount", parameters);

            if (dt == null || dt.Rows.Count == 0)
            {
                TempData["Message"] = "Something went wrong. Please try again.";
                TempData["MessageType"] = "error";
                return View(model);
            }

            DataRow row = dt.Rows[0];
            int loginSuccess = Convert.ToInt32(row["LoginSuccess"]);

            // ❌ LOGIN FAILED
            if (loginSuccess == 0)
            {
                TempData["Message"] = row["Message"]?.ToString() ?? "Login failed";
                TempData["MessageType"] = "error";
                return View(model);
            }

            // ✅ LOGIN SUCCESS

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, row["UserId"].ToString()),
        new Claim(ClaimTypes.Name, row["UserName"].ToString()),
        new Claim(ClaimTypes.Role, row["UserType"].ToString())
    };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            // ✅ ADD THIS (SUCCESS MESSAGE)
            TempData["Message"] = "Login successfully!";
            TempData["MessageType"] = "success";

            string userType = row["UserType"].ToString().Trim();

            if (userType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            else if (userType.Equals("Inventory", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Dashboard", new { area = "Inventory" });

            else if (userType.Equals("InvForSite", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Dashboard", new { area = "InvForSite" });

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            SqlParameter[] parameters =
            {
        new SqlParameter("@Action","REGISTER"),
        new SqlParameter("@UserName", model.UserName),
        new SqlParameter("@Password", model.Password),
        new SqlParameter("@UserType", model.UserType),
        new SqlParameter("@ReferenceType", model.ReferenceType),
        new SqlParameter("@DoctorId", model.DoctorId),
        new SqlParameter("@NurseId", model.NurseId)
    };

            DataTable dt = await _dbLayer.ExecuteSPAsync("sp_UserAccount", parameters);

            ViewData["Message"] = "User Registered Successfully";

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }


    }
}