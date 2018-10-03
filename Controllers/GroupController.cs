using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using wie_doet_de_afwas.Models;
using System.Collections.Generic;
using wie_doet_de_afwas.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace wie_doet_de_afwas
{
    public class GroupController : Controller
    {
        private readonly WDDAContext wDDAContext;

        public GroupController(WDDAContext wDDAContext) {
            this.wDDAContext = wDDAContext;
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateGroupViewModel createGroupViewModel) {
            var person = wDDAContext.Persons.First(
                (p) => p.UserName == HttpContext.User.Claims.First(
                    (c) => c.Type == ClaimTypes.NameIdentifier
                ).Value);

            var group = new Group();
            var groupMember = new GroupMember();
            groupMember.Person = person;
            groupMember.Group = group;
            groupMember.Administrator = true;
            group.Name = createGroupViewModel.Name;

            await wDDAContext.Groups.AddAsync(group);

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

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddGroupMember([FromBody] GroupMemberViewModel groupMemberViewModel)
        {
            VerifyIsGroupAdministrator(groupMemberViewModel.GroupId);

            var group = wDDAContext.Groups.First((g) => g.Id == groupMemberViewModel.GroupId);

            var groupMember = new GroupMember();
            groupMember.Administrator = groupMemberViewModel.Administrator;
            groupMember.Group = group;
            groupMember.Person = wDDAContext.Persons.First((p) => p.Id == groupMemberViewModel.PersonId);
            await wDDAContext.GroupMembers.AddAsync(groupMember);
            return Json(new {
                Succeeded = true
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteGroupMember([FromBody] GroupMemberViewModel groupMemberViewModel)
        {
            if (!VerifyIsGroupAdministrator(groupMemberViewModel.GroupId)) {
                return Unauthorized();
            }

            var groupMember = wDDAContext.GroupMembers.First((gm) =>
                gm.Group.Id == groupMemberViewModel.GroupId &&
                gm.Person.Id == groupMemberViewModel.PersonId
            );

            wDDAContext.GroupMembers.Remove(groupMember);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        private Person GetPerson()
        {
            return wDDAContext.Persons.First(
                (p) => p.UserName == HttpContext.User.Claims.First(
                    (c) => c.Type == ClaimTypes.NameIdentifier
                ).Value);
        }

        private bool VerifyIsGroupAdministrator(string groupId)
        {
            var person = GetPerson();

            return wDDAContext.GroupMembers.Any(
                (gm) =>
                    gm.Administrator &&
                    gm.Group.Id == groupId &&
                    gm.Person.Id == person.Id
            );
        }
    }
}