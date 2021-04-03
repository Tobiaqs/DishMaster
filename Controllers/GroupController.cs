using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using DishMaster.Models;
using System.Collections.Generic;
using DishMaster.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;
using Microsoft.EntityFrameworkCore;
using DishMaster.Data;

namespace DishMaster.Controllers
{
    public class GroupController : BaseController
    {
        public GroupController(DMContext dMContext) : base(dMContext)
        { }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult List()
        {
            var person = GetPerson();

            var groupMembers = dMContext.GroupMembers
                .Where((gm) => gm.Person == person)
                .Include((gm) => gm.Group);

            var groups = groupMembers
                .Select<GroupMember, ListGroupsViewModel>((gm) => new ListGroupsViewModel(gm.Group)).ToList();
            return SucceededJson(groups);
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string groupId)
        {
            if (!VerifyIsGroupMember(groupId))
            {
                return UnauthorizedJson();
            }

            var group = dMContext.Groups
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

            await dMContext.Groups.AddAsync(group);
            await dMContext.GroupMembers.AddAsync(groupMember);
            await dMContext.SaveChangesAsync();

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

            var group = dMContext.Groups.Single(g => g.Id == groupId);

            dMContext.Groups.Remove(group);
            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateGroupViewModel updateGroupViewModel)
        {
            if (!VerifyIsGroupAdministrator(updateGroupViewModel.GroupId)) {
                return UnauthorizedJson();
            }

            var group = dMContext.Groups.Single(g => g.Id == updateGroupViewModel.GroupId);

            group.Name = updateGroupViewModel.Name;

            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetGroupRoles([FromQuery, IsGuid] string groupId)
        {
            var groupMember = await dMContext.GroupMembers
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
            var groupMember = dMContext.GroupMembers
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
            var groupMember = dMContext.GroupMembers
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

            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DemoteGroupMember([FromQuery, IsGuid] string groupMemberId)
        {
            var groupMember = dMContext.GroupMembers
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

            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ResetGroupMemberScore([FromQuery, IsGuid] string groupMemberId)
        {
            var allGroupMembers = dMContext.GroupMembers
                .Include(gm => gm.Group);
            
            var groupMember = allGroupMembers.SingleOrDefault(gm => gm.Id == groupMemberId);

            if (groupMember == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(groupMember.Group.Id))
            {
                return UnauthorizedJson();
            }

            var otherGroupMembers = allGroupMembers.Where(gm => gm.Group == groupMember.Group && gm != groupMember);

            double average = otherGroupMembers.Sum(gm => gm.Score) / otherGroupMembers.Count();

            groupMember.Score = average;

            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateGroupMember([FromBody] UpdateGroupMemberViewModel updateGroupMemberViewModel)
        {
            var groupMember = dMContext.GroupMembers
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

            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddAnonymousGroupMember([FromBody] AddAnonymousGroupMemberViewModel addAnonymousGroupMemberViewModel)
        {
            if (!VerifyIsGroupAdministrator(addAnonymousGroupMemberViewModel.GroupId))
            {
                return UnauthorizedJson();
            }

            var group = dMContext.Groups
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

            await dMContext.GroupMembers.AddAsync(groupMember);
            await dMContext.SaveChangesAsync();
            
            return Json(new {
                Succeeded = true,
                GroupMemberId = groupMember.Id
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LeaveGroup([FromQuery, IsGuid] string groupId)
        {
            var groupMember = dMContext.GroupMembers.SingleOrDefault(
                gm => gm.Group.Id == groupId && gm.Person == GetPerson()
            );

            if (groupMember == null) {
                return NotFoundJson();
            }

            if (groupMember.Administrator)
            {
                return UnauthorizedJson();
            }

            dMContext.GroupMembers.Remove(groupMember);
            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteGroupMember([FromQuery, IsGuid] string groupMemberId)
        {
            var groupMember = dMContext.GroupMembers
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

            dMContext.GroupMembers.Remove(groupMember);
            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }
    }
}