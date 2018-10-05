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

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string groupId)
        {
            if (!VerifyIsGroupMember(groupId))
            {
                return Unauthorized();
            }

            var group = wDDAContext.Groups.First((g) => g.Id == groupId);

            var groupMembers = wDDAContext.GroupMembers.Where((gm) => gm.Group == group);

            return Json(new GroupViewModel(group, groupMembers));
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
                return Unauthorized();
            }

            var group = wDDAContext.Groups.First((g) => g.Id == groupId);

            var groupMembers = wDDAContext.GroupMembers.Where((gm) => gm.Group == group);

            var taskGroups = wDDAContext.TaskGroups.Where((tg) => tg.Group == group);

            var tasks = wDDAContext.Tasks.Where((t) => taskGroups.Contains(t.TaskGroup));

            var taskGroupRecords = wDDAContext.TaskGroupRecord.Where((tgr) => taskGroups.Contains(tgr.TaskGroup));

            wDDAContext.TaskGroupRecord.RemoveRange(taskGroupRecords);
            wDDAContext.Tasks.RemoveRange(tasks);
            wDDAContext.TaskGroups.RemoveRange(taskGroups);
            wDDAContext.GroupMembers.RemoveRange(groupMembers);
            wDDAContext.Groups.Remove(group);
            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateGroupViewModel updateGroupViewModel)
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

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetGroupMember([FromQuery, IsGuid] string groupMemberId)
        {
            var groupMember = wDDAContext.GroupMembers.FirstOrDefault((gm) => gm.Id == groupMemberId);

            if (groupMember == null)
            {
                return NotFound();
            }

            if (!VerifyIsGroupMember(groupMember.Group.Id))
            {
                return Unauthorized();
            }

            return Json(new GroupMemberViewModel(groupMember));
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteGroupMember([FromQuery, IsGuid] string groupMemberId)
        {
            var groupMember = wDDAContext.GroupMembers.FirstOrDefault(
                (gm) => gm.Id == groupMemberId
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