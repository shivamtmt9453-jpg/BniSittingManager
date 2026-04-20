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
                TempData["Message"] = "Unexpected error occurred.";
                TempData["MessageType"] = "error";
                return View(model);
            }

            DataRow row = dt.Rows[0];

            int loginSuccess = row["LoginSuccess"] != DBNull.Value
                ? Convert.ToInt32(row["LoginSuccess"])
                : 0;

            if (loginSuccess == 0)
            {
                TempData["Message"] = row["Message"]?.ToString() ?? "Login failed";
                TempData["MessageType"] = "error";
                return View(model);
            }

            // SAFE CLAIMS
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, row["UserId"]?.ToString() ?? ""),
        new Claim(ClaimTypes.Name, row["UserName"]?.ToString() ?? ""),
        new Claim(ClaimTypes.Role, row["UserType"]?.ToString() ?? "")
    };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            TempData["Message"] = "Login successfully!";
            TempData["MessageType"] = "success";

            string userType = row["UserType"]?.ToString()?.Trim() ?? "";

            return userType.ToLower() switch
            {
                "admin" => RedirectToAction("Index", "Dashboard", new { area = "Admin" }), 
                _ => RedirectToAction("Login", "Account")
            };
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