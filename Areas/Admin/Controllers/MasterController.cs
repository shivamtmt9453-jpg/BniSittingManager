using BniSittingManager.Areas.Admin.Controllers;
using BniSittingManager.Data;
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




    }
}
