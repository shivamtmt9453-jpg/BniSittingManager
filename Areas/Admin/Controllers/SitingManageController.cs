using BniSittingManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View();
        }


    }
}
