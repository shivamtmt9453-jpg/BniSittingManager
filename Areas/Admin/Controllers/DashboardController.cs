using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; 
using BniSittingManager.Controllers;
using System.Data;
using System.Text;
using BniSittingManager.Data;

namespace BniSittingManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IDbLayer _dbLayer;

        public DashboardController(IDbLayer dbLayer)
        {
            _dbLayer = dbLayer;
        }


        public IActionResult Index()
        {
            return View();
        }   

        public async Task<IActionResult> Logout()
        {
            // Clear all session data
            await HttpContext.SignOutAsync("MyCookieAuth"); 
            // Redirect to Login page
            return RedirectToAction("Login", "Account", new { area = "" });
        }



    }
}
