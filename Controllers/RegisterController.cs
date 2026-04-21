
using BniSittingManager.Data;
using BniSittingManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BniSittingManager.Controllers
{

    public class RegisterController : Controller
    {
          private readonly IDbLayer _dbLayer;

            public RegisterController(IDbLayer dbLayer)
            {
                _dbLayer = dbLayer;
            }


        /* ================= AUTO ID ================= */
        private async Task<string> GetNextBniMemberID()
        {
            string q = @"
    SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(BniMemberId,3,LEN(BniMemberId)) AS INT)),0)
    FROM bniUsers
    WHERE BniMemberId LIKE 'BM%'";

            int next = Convert.ToInt32(await _dbLayer.ExecuteScalarAsync(q)) + 1;

            return "BM" + next.ToString(); // No fixed 3 digit limit
        }

        /* ================= ADD ================= */
        [HttpGet]
            public async Task<IActionResult> BniMember()
            {
                BniUsers model = new();

                model.CreatedAt = DateTime.Now;
                model.Type = "BNI Member";
                model.BniMemberId = await GetNextBniMemberID();

                return View(model);
            }

            /* ================= SAVE ================= */
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> BniMember(BniUsers model)
            {
                if (!ModelState.IsValid)
                    return View(model);

                string action = model.UserId == 0 ? "INSERT" : "UPDATE";

                SqlParameter[] param =
                {
            new SqlParameter("@Action", action),
            new SqlParameter("@UserId", model.UserId),
           new SqlParameter("@BniMemberId", model.BniMemberId),  
            new SqlParameter("@Name", model.Name ?? ""),
            new SqlParameter("@Email", model.Email ?? ""),
            new SqlParameter("@Phone", model.Phone ?? ""),
            new SqlParameter("@CompanyName", model.CompanyName ?? ""),
            new SqlParameter("@Type","Member")
        };

                await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage", param);

                TempData["Message"] = model.UserId == 0
                    ? "User saved successfully!"
                    : "User updated successfully!";

                return RedirectToAction("BniMemberList");
            }


        private async Task<string> GetNextVisitorMemberID()
        {
            string q = @"
    SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(BniMemberId,3,LEN(BniMemberId)) AS INT)),0)
    FROM bniUsers
    WHERE BniMemberId LIKE 'VS%'";

            int next = Convert.ToInt32(await _dbLayer.ExecuteScalarAsync(q)) + 1;

            return "VS" + next.ToString();
        }

        /* ================= ADD ================= */
        [HttpGet]
        public async Task<IActionResult> BniVisitor()
        {
            BniUsers model = new();

            model.CreatedAt = DateTime.Now;
            model.Type = "BNI Visitor Member";
            model.BniMemberId = await GetNextVisitorMemberID();

            return View(model);
        }

        /* ================= SAVE ================= */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BniVisitor(BniUsers model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string action = model.UserId == 0 ? "INSERT" : "UPDATE";

            SqlParameter[] param =
            {
            new SqlParameter("@Action", action),
            new SqlParameter("@UserId", model.UserId),
           new SqlParameter("@BniMemberId", model.BniMemberId),
            new SqlParameter("@Name", model.Name ?? ""),
            new SqlParameter("@Email", model.Email ?? ""),
            new SqlParameter("@Phone", model.Phone ?? ""),
            new SqlParameter("@CompanyName", model.CompanyName ?? ""),
            new SqlParameter("@Type","Visitor")
        };

            await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage", param);

            TempData["Message"] = model.UserId == 0
                ? "User saved successfully!"
                : "User updated successfully!";

            return RedirectToAction("BniVisitorList");
        }
         
    }
}
