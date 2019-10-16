using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace GW2Integration.Server.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        [Route("{errorId}")]
        public JsonResult Index(int errorId)
        {
            var jsonResult = Json(new Dictionary<string, int>{{"error_id", errorId } });
            jsonResult.StatusCode = errorId;
            return jsonResult;
        }
    }
}
