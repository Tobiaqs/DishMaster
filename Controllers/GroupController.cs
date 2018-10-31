using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using wie_doet_de_afwas.Models;
using System.Collections.Generic;
using wie_doet_de_afwas.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;
using Microsoft.EntityFrameworkCore;

namespace wie_doet_de_afwas.Controllers
{
    public class GroupController : BaseController
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

            return SucceededJson(groups);
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string groupId)
        {
            if (!VerifyIsGroupMember(groupId))
            {
                return UnauthorizedJson();
            }

            var group = wDDAContext.Groups
                .Include(g => g.GroupMembers)
                    .ThenInclude((GroupMember gm) => gm.Person)
                .Single((g) => g.Id == groupId);

            return SucceededJson(new GroupViewModel(group));
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateGroupViewModel createGroupViewModel)
        {
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

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string groupId)
        {
            if (!VerifyIsGroupAdministrator(groupId))
            {
                return UnauthorizedJson();
            }

            var group = wDDAContext.Groups.Single(g => g.Id == groupId);

            wDDAContext.Groups.Remove(group);
            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateGroupViewModel updateGroupViewModel)
        {
            if (!VerifyIsGroupAdministrator(updateGroupViewModel.GroupId)) {
                return UnauthorizedJson();
            }

            var group = wDDAContext.Groups.Single(g => g.Id == updateGroupViewModel.GroupId);

            group.Name = updateGroupViewModel.Name;

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetGroupRoles([FromQuery, IsGuid] string groupId)
        {
            var groupMember = await wDDAContext.GroupMembers
                .Include(gm => gm.Group)
                    .ThenInclude(g => g.GroupMembers)
                .SingleOrDefaultAsync(gm => gm.Person == GetPerson() && gm.Group.Id == groupId);

            if (groupMember == null)
            {
                return UnauthorizedJson();
            }

            return SucceededJson(new GroupRolesViewModel(groupMember));
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetGroupMember([FromQuery, IsGuid] string groupMemberId)
        {
            var groupMember = wDDAContext.GroupMembers
                .Include(gm => gm.Group)
                .Include(gm => gm.Person)
                .SingleOrDefault(gm => gm.Id == groupMemberId);

            if (groupMember == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupMember(groupMember.Group.Id))
            {
                return UnauthorizedJson();
            }

            return SucceededJson(new GroupMemberViewModel(groupMember));
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PromoteGroupMember([FromQuery, IsGuid] string groupMemberId)
        {
            var groupMember = wDDAContext.GroupMembers
                .Include(gm => gm.Group)
                .Include(gm => gm.Person) // including for virtual property IsAnonymous
                .SingleOrDefault(gm => gm.Id == groupMemberId);

            if (groupMember == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(groupMember.Group.Id))
            {
                return UnauthorizedJson();
            }

            if (groupMember.IsAnonymous || groupMember.Administrator)
            {
                return FailedJson();
            }

            groupMember.Administrator = true;

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DemoteGroupMember([FromQuery, IsGuid] string groupMemberId)
        {
            var groupMember = wDDAContext.GroupMembers
                .Include(gm => gm.Group)
                .Include(gm => gm.Person) // including for virtual property IsAnonymous
                .SingleOrDefault(gm => gm.Id == groupMemberId);

            if (groupMember == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(groupMember.Group.Id))
            {
                return UnauthorizedJson();
            }

            if (groupMember.IsAnonymous || !groupMember.Administrator)
            {
                return FailedJson();
            }

            groupMember.Administrator = false;

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateGroupMember([FromBody] UpdateGroupMemberViewModel updateGroupMemberViewModel)
        {
            var groupMember = wDDAContext.GroupMembers
                .Include(gm => gm.Group)
                .SingleOrDefault(gm => gm.Id == updateGroupMemberViewModel.GroupMemberId);

            if (groupMember == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(groupMember.Group.Id))
            {
                return UnauthorizedJson();
            }

            groupMember.AbsentByDefault = updateGroupMemberViewModel.AbsentByDefault;

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddAnonymousGroupMember([FromBody] AddAnonymousGroupMemberViewModel addAnonymousGroupMemberViewModel)
        {
            if (!VerifyIsGroupAdministrator(addAnonymousGroupMemberViewModel.GroupId))
            {
                return UnauthorizedJson();
            }

            var group = wDDAContext.Groups
                .Include(g => g.GroupMembers)
                .Single(g => g.Id == addAnonymousGroupMemberViewModel.GroupId);

            double averageScore = 0;
            foreach (var gm in group.GroupMembers)
            {
                averageScore += gm.Score;
            }
            averageScore /= group.GroupMembers.Count;

            var groupMember = new GroupMember();
            groupMember.AnonymousName = addAnonymousGroupMemberViewModel.AnonymousName;
            groupMember.Group = group;
            groupMember.Score = averageScore;

            await wDDAContext.GroupMembers.AddAsync(groupMember);
            await wDDAContext.SaveChangesAsync();
            
            return Json(new {
                Succeeded = true,
                GroupMemberId = groupMember.Id
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LeaveGroup([FromQuery, IsGuid] string groupId)
        {
            var groupMember = wDDAContext.GroupMembers.SingleOrDefault(
                gm => gm.Group.Id == groupId && gm.Person == GetPerson()
            );

            if (groupMember == null) {
                return NotFoundJson();
            }

            if (groupMember.Administrator)
            {
                return UnauthorizedJson();
            }

            wDDAContext.GroupMembers.Remove(groupMember);
            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteGroupMember([FromQuery, IsGuid] string groupMemberId)
        {
            var groupMember = wDDAContext.GroupMembers
                .Include(gm => gm.Group)
                .Include(gm => gm.Person)
                .SingleOrDefault(gm => gm.Id == groupMemberId);

            if (groupMember == null) {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(groupMember.Group.Id)) {
                return UnauthorizedJson();
            }

            if (groupMember.Person == GetPerson())
            {
                return UnauthorizedJson();
            }

            wDDAContext.GroupMembers.Remove(groupMember);
            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }
    }
}