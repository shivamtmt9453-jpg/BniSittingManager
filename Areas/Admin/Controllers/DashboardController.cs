using BniSittingManager.Areas.Admin.Models;
using BniSittingManager.Controllers;
using BniSittingManager.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; 
using System.Data;
using System.Text;

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


        [HttpGet]
        public async Task<IActionResult> Index()
        {  
            var model = new DashboardViewModel();

            // Execute SP with Action parameter
            var dtDashboard = await _dbLayer.ExecuteSPAsync(
                "sp_GetDashboardData",
                new[] { new SqlParameter("@Action", "GetDashboardStats") }
            );

            if (dtDashboard.Rows.Count > 0)
            {
                var row = dtDashboard.Rows[0];

                model.TotalBniMember = row["TotalBniMember"] != DBNull.Value ? (int)row["TotalBniMember"] : 0;
                model.TotalBniVisiter = row["TotalBniVisiter"] != DBNull.Value ? (int)row["TotalBniVisiter"] : 0;
            }
             
            return View(model);
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
