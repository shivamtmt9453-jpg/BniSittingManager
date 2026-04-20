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

            /* ================= LIST ================= */
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

            /* ================= AUTO ID ================= */
            private async Task<string> GetNextBniMemberID()
            {
                string q = @"
        SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(BniMemberId,3,10) AS INT)),0)
        FROM bniUsers
        WHERE BniMemberId LIKE 'BM[0-9][0-9][0-9]'";

                int next = Convert.ToInt32(await _dbLayer.ExecuteScalarAsync(q)) + 1;

                return "BM" + next.ToString("D3");
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
            new SqlParameter("@BniMemberId", DBNull.Value), // 🔥 SP generate karega
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

                return RedirectToAction("BniMemberList");
            }

            /* ================= EDIT ================= */
            public async Task<IActionResult> EditBniUser(int id)
            {
                SqlParameter[] param =
                {
            new SqlParameter("@Action", "GETBYID"),
            new SqlParameter("@UserId", id)
        };

                var dt = await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage", param);

                BniUsers model = new();

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

                return View("BniMember", model);
            }

            /* ================= DELETE ================= */
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


        private async Task<string> GetNextVisitorMemberID()
        {
            string q = @"
        SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(BniMemberId,3,10) AS INT)),0)
        FROM bniUsers
        WHERE BniMemberId LIKE 'VS[0-9][0-9][0-9]'";

            int next = Convert.ToInt32(await _dbLayer.ExecuteScalarAsync(q)) + 1;

            return "VS" + next.ToString("D3");
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
            new SqlParameter("@BniMemberId", DBNull.Value), // 🔥 SP generate karega
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

            return RedirectToAction("BniVisitorList");
        }


        public async Task<IActionResult> EditBniVisitor(int id)
        {
            SqlParameter[] param =
            {
            new SqlParameter("@Action", "GETBYID"),
            new SqlParameter("@UserId", id)
        };

            var dt = await _dbLayer.ExecuteSPAsync("sp_BniUsers_Manage", param);

            BniUsers model = new();

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

            return View("BniMember", model);
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
