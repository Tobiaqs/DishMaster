using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wie_doet_de_afwas.Models;
using wie_doet_de_afwas.ViewModels;

namespace wie_doet_de_afwas.Controllers
{
    public class InvitationController : TokenAuthBaseController
    {
        public InvitationController(WDDAContext wDDAContext) : base(wDDAContext)
        {}

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationViewModel acceptInvitationViewModel)
        {
            var group = wDDAContext.Groups.FirstOrDefault((g) =>
                g.InvitationSecret == acceptInvitationViewModel.InvitationSecret &&
                g.InvitationExpiration > System.DateTime.UtcNow
            );

            if (group == null)
            {
                return NotFound();
            }

            group.InvitationSecret = null;

            var groupMember = new GroupMember();
            groupMember.Administrator = false;
            groupMember.Group = group;
            groupMember.Person = GetPerson();

            await wDDAContext.GroupMembers.AddAsync(groupMember);
            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetInvitationSecret([FromBody] GetInvitationViewModel getInvitationViewModel)
        {
            if (!VerifyIsGroupAdministrator(getInvitationViewModel.GroupId)) {
                return Unauthorized();
            }

            var group = wDDAContext.Groups.First((g) => g.Id == getInvitationViewModel.GroupId);
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