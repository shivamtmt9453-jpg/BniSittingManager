using BniSittingManager.Areas.Admin.Models;
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
            try
            {
                await _dbLayer.ExecuteSPAsyncgenerate(
     "sp_GenerateSixRoundsSeating",
     null
 );

                TempData["Message"] = "All 6 Rounds generated successfully!";
                TempData["MessageType"] = "success";
            } 
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("Index");
        }

        // ================= ROUND WISE VIEW =================
        public async Task<IActionResult> RoundView(int roundId)
        {
            var list = new List<RoundViewVM>();

            var dt = await _dbLayer.ExecuteSPAsync("sp_GetRoundSeating",
                new SqlParameter[]
                {
            new SqlParameter("@RoundId", roundId)
                });

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new RoundViewVM
                {
                    TableName = r["TableName"].ToString() ?? string.Empty,
                    SeatNumber = r["SeatNumber"].ToString() ?? string.Empty,
                    UserName = r["UserName"].ToString() ?? string.Empty,
                    Type = r["Type"].ToString() ?? string.Empty,
                    SeatName = r["SeatName"].ToString() ?? string.Empty,
                    MemberId = r["BniMemberId"].ToString() ?? string.Empty
                });
            }
            ViewBag.RoundId = roundId;
            return View(list);
        }

        public async Task<IActionResult> ExportExcelRoundwise(int roundId)
        {
            try
            {
                var dt = await _dbLayer.ExecuteSPAsync("sp_GetAllRoundspdfSeating", new SqlParameter[] {  new SqlParameter("@Action", "BYROUND"),
            new SqlParameter("@RoundID", roundId) });

                var file = _dbLayer.ExportToExcel(dt, "BNI_Seating_Report.xlsx");

                //TempData["Message"] = "Excel Exported Successfully!";
                //TempData["MessageType"] = "success";

                return File(file,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "BNI_Round_Seating_Report.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }
        // ================= ALL ROUNDS REPORT =================
        public async Task<IActionResult> AllRoundsReport()
        {
            var list = new List<AllRoundsReportVM>();

            var dt = await _dbLayer.ExecuteSPAsync("sp_GetAllRoundsSeating", new SqlParameter[] { });

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new AllRoundsReportVM
                {
                    RoundName = r["RoundName"].ToString(),
                    TableName = r["TableName"].ToString(),
                    SeatNumber = r["SeatNumber"].ToString(),
                    UserName = r["Name"].ToString(),
                    Type = r["Type"].ToString(),
                    SeatName = r["SeatName"].ToString(),
                    MemberId = r["BniMemberId"].ToString()
                });
            }

            return View(list);
        }

        // ================= EXPORT EXCEL =================
        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                var dt = await _dbLayer.ExecuteSPAsync("sp_GetAllRoundspdfSeating", new SqlParameter[] { new SqlParameter("@Action", "ALL"),
            new SqlParameter("@RoundID", DBNull.Value) });

                var file = _dbLayer.ExportToExcel(dt, "BNI_Seating_Report.xlsx");

                //TempData["Message"] = "Excel Exported Successfully!";
                //TempData["MessageType"] = "success";

                return File(file,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "BNI_Seating_Report.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["MessageType"] = "error";
               return RedirectToAction("Index");
           }
        }



        // ================= Unseated Member list=================
        public async Task<IActionResult> UnseatedMemberlist()
        {
            var list = new List<UnseatedMemberVM>();

            var dt = await _dbLayer.ExecuteSPAsync(
                "sp_GetUnseatedMemberList",
                new SqlParameter[]
                {
            new SqlParameter("@Action", "List")
                });

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new UnseatedMemberVM
                {
                    RoundID = Convert.ToInt32(r["RoundID"]),
                    RoundName = "Round " + r["RoundID"],

                    UserName = r["Name"].ToString(),
                    MemberId = r["BniMemberId"].ToString(),
                    Phone = r["Phone"].ToString(),
                    Email = r["Email"].ToString(),

                    Type = "Unseated"
                });
            }

            return View(list);
        }

        // ================= EXPORT EXCEL =================
        public async Task<IActionResult> ExportExcelUnseatedMember()
        {
            try
            {
                var dt = await _dbLayer.ExecuteSPAsync("sp_GetUnseatedMemberList", new SqlParameter[] { new SqlParameter("@Action", "Listpdf") });

                var file = _dbLayer.ExportToExcel(dt, "BNI_Seating_Report.xlsx");

                //TempData["Message"] = "Excel Exported Successfully!";
                //TempData["MessageType"] = "success";

                return File(file,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "BNI_UnSeatedMember_Report.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }

         
    }
}