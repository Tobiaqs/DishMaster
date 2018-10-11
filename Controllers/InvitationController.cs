using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wie_doet_de_afwas.Models;
using wie_doet_de_afwas.ViewModels;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.Controllers
{
    public class InvitationController : BaseController
    {
        public InvitationController(WDDAContext wDDAContext) : base(wDDAContext)
        {}

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Accept([FromQuery, IsGuid] string invitationSecret)
        {
            var group = wDDAContext.Groups.SingleOrDefault((g) =>
                g.InvitationSecret == invitationSecret &&
                g.InvitationExpiration > System.DateTime.UtcNow
            );

            if (group == null)
            {
                return NotFoundJson();
            }

            if (VerifyIsGroupMember(group.Id))
            {
                return UnauthorizedJson();
            }

            group.InvitationSecret = null;

            var groupMember = new GroupMember();
            groupMember.Administrator = false;
            groupMember.Group = group;
            groupMember.Person = GetPerson();

            await wDDAContext.GroupMembers.AddAsync(groupMember);
            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true,
                GroupId = group.Id
            });
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetSecret([FromQuery, IsGuid] string groupId)
        {
            if (!VerifyIsGroupAdministrator(groupId)) {
                return UnauthorizedJson();
            }

            var group = wDDAContext.Groups.Single((g) => g.Id == groupId);
            group.InvitationExpiration = System.DateTime.UtcNow.AddDays(1);
            group.InvitationSecret = System.Guid.NewGuid().ToString();

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                InvitationSecret = group.InvitationSecret,
                Succeeded = true
            });
        }
    }
}