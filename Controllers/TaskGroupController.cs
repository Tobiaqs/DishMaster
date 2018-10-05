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
    public class TaskGroupController : TokenAuthBaseController
    {
        public TaskGroupController(WDDAContext wDDAContext) : base(wDDAContext)
        { }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult List([FromQuery, IsGuid] string groupId)
        {
            var groupMember = wDDAContext.GroupMembers.FirstOrDefault((gm) =>
                gm.Group.Id == groupId &&
                gm.Person == GetPerson()
            );

            if (groupMember == null) {
                return Unauthorized();
            }

            return Json(new ListTaskGroupsViewModel(groupMember.Group.TaskGroups));
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string taskGroupId)
        {
            var taskGroup = wDDAContext.TaskGroups.FirstOrDefault((tg) => tg.Id == taskGroupId);

            if (taskGroup == null)
            {
                return NotFound();
            }

            if (!VerifyIsGroupMember(taskGroup.Group.Id))
            {
                return Unauthorized();
            }

            return Json(new TaskGroupViewModel(taskGroup));
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateTaskGroupViewModel createTaskGroupViewModel)
        {
            if (!VerifyIsGroupAdministrator(createTaskGroupViewModel.GroupId))
            {
                return Unauthorized();
            }

            var group = wDDAContext.Groups.First((g) => g.Id == createTaskGroupViewModel.GroupId);

            var taskGroup = new TaskGroup();
            taskGroup.Name = createTaskGroupViewModel.Name;
            taskGroup.Group = group;

            group.TaskGroups = group.TaskGroups.Append(taskGroup);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true,
                TaskGroupId = taskGroup.Id
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string taskGroupId)
        {
            var taskGroup = wDDAContext.TaskGroups.FirstOrDefault((tg) => tg.Id == taskGroupId);

            if (taskGroup == null)
            {
                return NotFound();
            }

            if (!VerifyIsGroupAdministrator(taskGroup.Group.Id))
            {
                return Unauthorized();
            }

            taskGroup.Group.TaskGroups = taskGroup.Group.TaskGroups.Where((tg) => tg != taskGroup);

            wDDAContext.TaskGroups.Remove(taskGroup);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskGroupViewModel updateTaskGroupViewModel)
        {
            var taskGroup = wDDAContext.TaskGroups.FirstOrDefault((tg) => tg.Id == updateTaskGroupViewModel.TaskGroupId);

            // find out whether the actor is an admin of the group that holds this taskgroup
            var groupMember = wDDAContext.GroupMembers.FirstOrDefault((gm) =>
                gm.Group.TaskGroups.Contains(taskGroup) &&
                gm.Person == GetPerson() &&
                gm.Administrator
            );

            if (groupMember == null)
            {
                return Unauthorized();
            }

            taskGroup.Name = updateTaskGroupViewModel.Name;

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }
    }
}