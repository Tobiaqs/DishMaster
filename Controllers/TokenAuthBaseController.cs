using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.Controllers
{
    public abstract class TokenAuthBaseController : Controller
    {
        protected readonly WDDAContext wDDAContext;
        public TokenAuthBaseController(WDDAContext wDDAContext)
        {
            this.wDDAContext = wDDAContext;
        }

        protected Person GetPerson()
        {
            return wDDAContext.Persons.First(
                (p) => p.UserName == HttpContext.User.Claims.First(
                    (c) => c.Type == ClaimTypes.NameIdentifier
                ).Value);
        }

        protected bool VerifyIsGroupAdministrator(string groupId)
        {
            var person = GetPerson();

            return wDDAContext.GroupMembers.Any(
                (gm) =>
                    gm.Administrator &&
                    gm.Group.Id == groupId &&
                    gm.Person == person
            );
        }
    }
}