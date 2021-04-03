using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DishMaster.Annotations;
using DishMaster.Models;
using DishMaster.ViewModels;
using DishMaster.Data;

namespace DishMaster.Controllers
{
    public class TaskGroupController : BaseController
    {
        public TaskGroupController(DMContext dMContext) : base(dMContext)
        { }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ListTaskGroupRecords([FromBody] ListTaskGroupRecordsViewModel listTaskGroupRecordsViewModel)
        {
            var taskGroup = dMContext.TaskGroups
                .Include(tg => tg.Group)
                .SingleOrDefault(tg => tg.Id == listTaskGroupRecordsViewModel.TaskGroupId);

            if (taskGroup == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupMember(taskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            var taskGroupRecords = dMContext.TaskGroupRecords
                .Where(tgr => tgr.TaskGroup == taskGroup)
                .OrderByDescending(tgr => tgr.Date);

            if (!listTaskGroupRecordsViewModel.Superficial)
            {
                var taskGroupRecordsExtended = taskGroupRecords
                    .Include(tgr => tgr.PresentGroupMembers)
                        .ThenInclude((PresentGroupMember pgm) => pgm.GroupMember)
                    .Include(tgr => tgr.TaskGroupMemberLinks)
                        .ThenInclude((TaskGroupMemberLink link) => link.Task)
                    .Include(tgr => tgr.TaskGroupMemberLinks)
                        .ThenInclude((TaskGroupMemberLink link) => link.GroupMember);

                return SucceededJson(new TaskGroupRecordsViewModel(
                    taskGroupRecordsExtended
                        .Skip(listTaskGroupRecordsViewModel.Offset)
                        .Take(listTaskGroupRecordsViewModel.Count),
                    listTaskGroupRecordsViewModel.Superficial
                ));
            }
            else
            {
                return SucceededJson(new TaskGroupRecordsViewModel(
                    taskGroupRecords
                        .Skip(listTaskGroupRecordsViewModel.Offset)
                        .Take(listTaskGroupRecordsViewModel.Count),
                    listTaskGroupRecordsViewModel.Superficial
                ));
            }
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult List([FromQuery, IsGuid] string groupId)
        {
            var groupMember = dMContext.GroupMembers
                .Include(gm => gm.Group)
                .ThenInclude(g => g.TaskGroups)
                .SingleOrDefault(gm =>
                    gm.Group.Id == groupId &&
                    gm.Person == GetPerson()
                );

            if (groupMember == null) {
                return UnauthorizedJson();
            }

            return SucceededJson(new ListTaskGroupsViewModel(groupMember.Group.TaskGroups));
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string taskGroupId)
        {
            var taskGroup = dMContext.TaskGroups
                .Include(tg => tg.Group)
                .Include(tg => tg.Tasks)
                .SingleOrDefault(tg => tg.Id == taskGroupId);

            if (taskGroup == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupMember(taskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            return SucceededJson(new TaskGroupViewModel(taskGroup));
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateTaskGroupViewModel createTaskGroupViewModel)
        {
            if (!VerifyIsGroupAdministrator(createTaskGroupViewModel.GroupId))
            {
                return UnauthorizedJson();
            }

            var group = dMContext.Groups.Single(g => g.Id == createTaskGroupViewModel.GroupId);

            var taskGroup = new TaskGroup();
            taskGroup.Name = createTaskGroupViewModel.Name;
            taskGroup.Group = group;

            await dMContext.TaskGroups.AddAsync(taskGroup);
            await dMContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true,
                TaskGroupId = taskGroup.Id
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string taskGroupId)
        {
            var taskGroup = dMContext.TaskGroups
                .Include(tg => tg.Group)
                .SingleOrDefault(tg => tg.Id == taskGroupId);

            if (taskGroup == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(taskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            taskGroup.Group.TaskGroups.Remove(taskGroup);

            dMContext.TaskGroups.Remove(taskGroup);

            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskGroupViewModel updateTaskGroupViewModel)
        {
            var taskGroup = dMContext.TaskGroups.SingleOrDefault(tg => tg.Id == updateTaskGroupViewModel.TaskGroupId);

            // find out whether the actor is an admin of the group that holds this taskgroup
            var groupMember = dMContext.GroupMembers.SingleOrDefault(gm =>
                gm.Group.TaskGroups.Contains(taskGroup) &&
                gm.Person == GetPerson() &&
                gm.Administrator
            );

            if (groupMember == null)
            {
                return UnauthorizedJson();
            }

            taskGroup.Name = updateTaskGroupViewModel.Name;

            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }
    }
}