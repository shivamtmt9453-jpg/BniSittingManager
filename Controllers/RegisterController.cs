using BniSittingManager.Areas.Admin.Controllers;
using BniSittingManager.Data;
using Microsoft.AspNetCore.Mvc;

namespace BniSittingManager.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly IDbLayer _dbLayer;

        public RegisterController(IDbLayer dbLayer)
        {
            _dbLayer = dbLayer;
        }

        public IActionResult BniMember()
        {
            return View();
        }
        public IActionResult BniVisitor()
        {
            return View();
        }


    }
}
