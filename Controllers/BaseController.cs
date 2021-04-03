using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using DishMaster.Models;
using DishMaster.Data;

namespace DishMaster.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly DMContext dMContext;
        public BaseController(DMContext dMContext)
        {
            this.dMContext = dMContext;
        }

        protected Person GetPerson()
        {
            string username = HttpContext.User.Claims.First(
                    c => c.Type == ClaimTypes.NameIdentifier
                ).Value;
            return dMContext.Persons.Single(
                p => p.UserName == username);
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

            return dMContext.GroupMembers.Any(
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