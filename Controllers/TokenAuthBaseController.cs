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
                (gm) =>
                    gm.Group.Id == groupId &&
                    gm.Person == person &&
                    (!mustBeAdmin || gm.Administrator)
            );
        }
    }
}