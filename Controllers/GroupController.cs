using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using wie_doet_de_afwas.Models;
using System.Collections.Generic;
using wie_doet_de_afwas.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace wie_doet_de_afwas.Controllers
{
    public class GroupController : TokenAuthBaseController
    {
        public GroupController(WDDAContext wDDAContext) : base(wDDAContext)
        { }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult List()
        {
            var person = GetPerson();

            var groups = wDDAContext.GroupMembers
                .Where((gm) => gm.Person == person)
                .Select<GroupMember, ListGroupsViewModel>((gm) => new ListGroupsViewModel(gm.Group));

            return Json(groups);
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateGroupViewModel createGroupViewModel) {
            var person = GetPerson();

            var group = new Group();
            var groupMember = new GroupMember();

            groupMember.Person = person;
            groupMember.Group = group;
            groupMember.Administrator = true;

            group.Name = createGroupViewModel.Name;

            await wDDAContext.Groups.AddAsync(group);
            await wDDAContext.GroupMembers.AddAsync(groupMember);
            await wDDAContext.SaveChangesAsync();

            return Json(new {
                GroupId = group.Id,
                Succeeded = true
            });
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateGroupName([FromBody] UpdateGroupViewModel updateGroupViewModel)
        {
            if (!VerifyIsGroupAdministrator(updateGroupViewModel.GroupId)) {
                return Unauthorized();
            }

            var group = wDDAContext.Groups.First((g) => g.Id == updateGroupViewModel.GroupId);

            group.Name = updateGroupViewModel.Name;

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteGroupMember([FromBody] DeleteGroupMemberViewModel deleteGroupMemberViewModel)
        {
            var groupMember = wDDAContext.GroupMembers.FirstOrDefault(
                (gm) => gm.Id == deleteGroupMemberViewModel.GroupMemberId
            );

            if (groupMember == null) {
                return NotFound();
            }

            if (!VerifyIsGroupAdministrator(groupMember.Group.Id)) {
                return Unauthorized();
            }

            wDDAContext.GroupMembers.Remove(groupMember);
            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }
    }
}