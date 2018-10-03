using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.Controllers
{
    public class TaskController : Controller
    {
        private readonly WDDAContext wDDAContext;
        public TaskController(WDDAContext wDDAContext)
        {
            this.wDDAContext = wDDAContext;
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Cheese() {
            var currentUser = HttpContext.User;
            var claim = currentUser.Claims.First((c) => c.Type == ClaimTypes.NameIdentifier);
            var person = wDDAContext.Persons.First((p) => p.UserName == claim.Value);

            return Json(new {
                UserName = person.UserName,
                Email = person.Email
            });
        }
    }
}
