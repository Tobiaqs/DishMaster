using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
            var group = wDDAContext.Groups
                .Include(g => g.GroupMembers)
                .SingleOrDefault((g) =>
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

            float averageScore = 0;
            foreach (var gm in group.GroupMembers)
            {
                averageScore += gm.Score;
            }
            averageScore /= group.GroupMembers.Count;

            group.InvitationSecret = null;

            var groupMember = new GroupMember();
            groupMember.Administrator = false;
            groupMember.Group = group;
            groupMember.Person = GetPerson();
            groupMember.Score = averageScore;

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

            var group = wDDAContext.Groups.Single(g => g.Id == groupId);
            if (group.InvitationExpiration < System.DateTime.UtcNow || group.InvitationSecret == null)
            {
                group.InvitationSecret = System.Guid.NewGuid().ToString();
            }
            group.InvitationExpiration = System.DateTime.UtcNow.AddDays(1);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                InvitationSecret = group.InvitationSecret,
                Succeeded = true
            });
        }
    }
}