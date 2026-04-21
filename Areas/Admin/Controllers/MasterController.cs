using BniSittingManager.Areas.Admin.Controllers;
using BniSittingManager.Data;
using BniSittingManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;


namespace BniSittingManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] 
    public class MasterController : BaseController
    {
        private readonly IDbLayer _dbLayer;

        public MasterController(IDbLayer dbLayer)
        {
            _dbLayer = dbLayer;
        }



        /* ================= LIST ================= */
        public async Task<IActionResult> BniMemberList(string search)
        {
            SqlParameter[] param =
            {
            new SqlParameter("@Action", "MemberLIST"),
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage",
            new[]
            {
            new SqlParameter("@Action","DELETE"),
            new SqlParameter("@UserId",id)
            });

            return RedirectToAction("BniMemberList");
        }



        public async Task<IActionResult> BniVisitorList(string search)
        {
            SqlParameter[] param =
            {
       new SqlParameter("@Action", "VisitorLIST"),
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

        /* ================= DELETE ================= */
        [HttpPost]
        public async Task<IActionResult> VisitorDelete(int id)
        {
            await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage",
            new[]
            {
     new SqlParameter("@Action","DELETE"),
     new SqlParameter("@UserId",id)
            });

            return RedirectToAction("BniMemberList");
        }



    }

}