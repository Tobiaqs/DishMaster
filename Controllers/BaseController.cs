using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly WDDAContext wDDAContext;
        public BaseController(WDDAContext wDDAContext)
        {
            this.wDDAContext = wDDAContext;
        }

        protected Person GetPerson()
        {
            return wDDAContext.Persons.Single(
                p => p.UserName == HttpContext.User.Claims.First(
                    c => c.Type == ClaimTypes.NameIdentifier
                ).Value);
        }

        protected bool VerifyIsGroupAdministrator(string groupId)
        {
            return VerifyIsGroupMember(groupId, true);
        }

        protected bool VerifyIsGroupMember(string groupId)
        {
            return VerifyIsGroupMember(groupId, false);
        }

        protected bool VerifyIsGroupMember(string groupId, bool mustBeAdmin)
        {
            var person = GetPerson();

            return wDDAContext.GroupMembers.Any(
                gm =>
                    gm.Group.Id == groupId &&
                    gm.Person == person &&
                    (!mustBeAdmin || gm.Administrator)
            );
        }

        protected IActionResult SucceededJson(object data)
        {
            return Json(new {
                Succeeded = true,
                Payload = data
            });
        }

        protected IActionResult SucceededJson()
        {
            return Json(new {
                Succeeded = true
            });
        }

        protected IActionResult FailedJson()
        {
            return Json(new {
                Succeeded = false
            });
        }

        protected IActionResult UnauthorizedJson()
        {
            return Json(new {
                Succeeded = false,
                HttpError = 401
            });
        }

        protected IActionResult NotFoundJson()
        {
            return Json(new {
                Succeeded = false,
                HttpError = 404
            });
        }
    }
}