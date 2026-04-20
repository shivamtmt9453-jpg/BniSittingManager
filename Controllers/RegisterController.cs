using BniSittingManager.Areas.Admin.Controllers;
using BniSittingManager.Data;
using BniSittingManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BniSittingManager.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly IDbLayer _dbLayer;

        public RegisterController(IDbLayer dbLayer)
        {
            _dbLayer = dbLayer;
        }

        public IActionResult BniMemberList()
        {
            return View();
        }



        /* ================= LIST PAGE ================= */
        public async Task<IActionResult> BniMemberList(string search)
        {
            SqlParameter[] param =
            {
            new SqlParameter("@Action", "LIST"),
            new SqlParameter("@Search", search ?? (object)DBNull.Value)
        };

            var dt = await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage", param);

            List<BniUsers> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new BniUsers
                {
                    UserId = Convert.ToInt32(row["UserId"]),
                    BniMemberId = row["BniMemberId"]?.ToString(),
                    Name = row["Name"]?.ToString(),
                    Email = row["Email"]?.ToString(),
                    Phone = row["Phone"]?.ToString(),
                    CompanyName = row["CompanyName"]?.ToString(),
                    Type = row["Type"]?.ToString(),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"])
                });
            }

            return View(list);
        }

        /* ================= ADD ================= */
        [HttpGet]
        public IActionResult AddBniUser()
        {
            BniUsers model = new();
            model.CreatedAt = DateTime.Now;
            return View(model);
        }

        /* ================= SAVE (INSERT + UPDATE) ================= */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BniMember(BniUsers model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddBniUser", model);
            }

            string action = model.UserId == 0 ? "INSERT" : "UPDATE";

            SqlParameter[] param =
            {
            new SqlParameter("@Action", action),
            new SqlParameter("@UserId", model.UserId),
            new SqlParameter("@BniMemberId", model.BniMemberId ?? ""),
            new SqlParameter("@Name", model.Name ?? ""),
            new SqlParameter("@Email", model.Email ?? ""),
            new SqlParameter("@Phone", model.Phone ?? ""),
            new SqlParameter("@CompanyName", model.CompanyName ?? ""),
            new SqlParameter("@Type", model.Type ?? "")
        };

            await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage", param);

            TempData["Message"] = model.UserId == 0
                ? "User saved successfully!"
                : "User updated successfully!";

            TempData["MessageType"] = "success";

            return RedirectToAction("BniMemberList");
        }

        /* ================= EDIT ================= */
        [HttpGet]
        public async Task<IActionResult> EditBniUser(int id)
        {
            BniUsers model = new();

            SqlParameter[] param =
            {
            new SqlParameter("@Action", "GETBYID"),
            new SqlParameter("@UserId", id)
        };

            var dt = await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage", param);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                model.UserId = Convert.ToInt32(row["UserId"]);
                model.BniMemberId = row["BniMemberId"]?.ToString();
                model.Name = row["Name"]?.ToString();
                model.Email = row["Email"]?.ToString();
                model.Phone = row["Phone"]?.ToString();
                model.CompanyName = row["CompanyName"]?.ToString();
                model.Type = row["Type"]?.ToString();
                model.CreatedAt = Convert.ToDateTime(row["CreatedAt"]);
            }

            return View("AddBniUser", model); // reuse same form
        }

        /* ================= DELETE ================= */
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            SqlParameter[] param =
            {
            new SqlParameter("@Action", "DELETE"),
            new SqlParameter("@UserId", id)
        };

            await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage", param);

            TempData["Message"] = "User deleted successfully!";
            TempData["MessageType"] = "success";

            return RedirectToAction("BniMemberList");
        }

    
        public IActionResult BniVisitor()
        {
            return View();
        }


    }
}
