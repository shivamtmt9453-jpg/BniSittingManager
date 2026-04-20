using BniSittingManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BniSittingManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SitingManageController : BaseController
    {
        private readonly IDbLayer _dbLayer;

        public SitingManageController(IDbLayer dbLayer)
        {
            _dbLayer = dbLayer;
        }

        // ================= DASHBOARD =================
        public IActionResult Index()
        {
            return View();
        }

        // ================= GENERATE 6 ROUNDS =================
        [HttpPost]
        public async Task<IActionResult> GenerateRounds()
        {
            await _dbLayer.ExecuteSPAsync("sp_GenerateSixRoundsSeating", new SqlParameter[] { });

            TempData["Message"] = "6 Rounds Generated Successfully in One Click!";
            TempData["MessageType"] = "success";

            return RedirectToAction("Index");
        }
        // ================= ROUND WISE VIEW =================
        public async Task<IActionResult> RoundView(int roundId)
        {
            var list = new List<dynamic>();

            try
            {
                var dt = await _dbLayer.ExecuteSPAsync("sp_GetRoundSeating",
                    new SqlParameter[]
                    {
                        new SqlParameter("@RoundId", roundId)
                    });

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new
                    {
                        TableName = r["TableName"].ToString(),
                        SeatNumber = r["SeatNumber"].ToString(),
                        UserName = r["UserName"].ToString(),
                        Type = r["Type"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["MessageType"] = "error";
            }

            return View(list);
        }

        // ================= ALL ROUNDS REPORT =================
        public async Task<IActionResult> AllRoundsReport()
        {
            var list = new List<dynamic>();

            try
            {
                var dt = await _dbLayer.ExecuteSPAsync("sp_GetAllRoundsSeating", new SqlParameter[] { });

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new
                    {
                        RoundName = r["RoundName"].ToString(),
                        TableName = r["TableName"].ToString(),
                        SeatNumber = r["SeatNumber"].ToString(),
                        UserName = r["Name"].ToString(),
                        Type = r["Type"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["MessageType"] = "error";
            }

            return View(list);
        }

        // ================= EXPORT EXCEL =================
        //public async Task<IActionResult> ExportExcel()
        //{
        //    try
        //    {
        //        var dt = await _dbLayer.ExecuteSPAsync("sp_GetAllRoundsSeating", new SqlParameter[] { });

        //        var file = _dbLayer.ExportToExcel(dt, "BNI_Seating_Report.xlsx");

        //        TempData["Message"] = "Excel Exported Successfully!";
        //        TempData["MessageType"] = "success";

        //        return File(file,
        //            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //            "BNI_Seating_Report.xlsx");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Message"] = ex.Message;
        //        TempData["MessageType"] = "error";
        //        return RedirectToAction("Index");
        //    }
        //}










    }
}