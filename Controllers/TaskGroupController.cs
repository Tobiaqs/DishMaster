using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wie_doet_de_afwas.Annotations;
using wie_doet_de_afwas.Models;
using wie_doet_de_afwas.ViewModels;

namespace wie_doet_de_afwas.Controllers
{
    public class TaskGroupController : BaseController
    {
        public TaskGroupController(WDDAContext wDDAContext) : base(wDDAContext)
        { }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult List([FromQuery, IsGuid] string groupId)
        {
            var groupMember = wDDAContext.GroupMembers.SingleOrDefault((gm) =>
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
            var taskGroup = wDDAContext.TaskGroups.SingleOrDefault((tg) => tg.Id == taskGroupId);

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

            var group = wDDAContext.Groups.Single((g) => g.Id == createTaskGroupViewModel.GroupId);

            var taskGroup = new TaskGroup();
            taskGroup.Name = createTaskGroupViewModel.Name;
            taskGroup.Group = group;

            group.TaskGroups.Add(taskGroup);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true,
                TaskGroupId = taskGroup.Id
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string taskGroupId)
        {
            var taskGroup = wDDAContext.TaskGroups.SingleOrDefault((tg) => tg.Id == taskGroupId);

            if (taskGroup == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(taskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            taskGroup.Group.TaskGroups.Remove(taskGroup);

            wDDAContext.TaskGroups.Remove(taskGroup);

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskGroupViewModel updateTaskGroupViewModel)
        {
            var taskGroup = wDDAContext.TaskGroups.SingleOrDefault((tg) => tg.Id == updateTaskGroupViewModel.TaskGroupId);

            // find out whether the actor is an admin of the group that holds this taskgroup
            var groupMember = wDDAContext.GroupMembers.SingleOrDefault((gm) =>
                gm.Group.TaskGroups.Contains(taskGroup) &&
                gm.Person == GetPerson() &&
                gm.Administrator
            );

            if (groupMember == null)
            {
                return UnauthorizedJson();
            }

            taskGroup.Name = updateTaskGroupViewModel.Name;

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }
    }
}