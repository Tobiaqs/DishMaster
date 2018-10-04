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
    public class TaskController : TokenAuthBaseController
    {
        public TaskController(WDDAContext wDDAContext) : base(wDDAContext)
        { }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string taskId)
        {
            var task = wDDAContext.Tasks.FirstOrDefault((t) => t.Id == taskId);
            
            if (task == null)
            {
                return NotFound();
            }

            var groupMember = wDDAContext.GroupMembers.FirstOrDefault((gm) =>
                gm.Group == task.TaskGroup.Group &&
                gm.Person == GetPerson());

            if (groupMember == null)
            {
                return Unauthorized();
            }

            return Json(task);
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateTaskViewModel createTaskViewModel)
        {
            var taskGroup = wDDAContext.TaskGroups.FirstOrDefault((tg) => tg.Id == createTaskViewModel.TaskGroupId);

            if (!VerifyIsGroupAdministrator(taskGroup.Group.Id))
            {
                return Unauthorized();
            }

            var task = new Models.Task();
            task.Name = createTaskViewModel.Name;
            task.Bounty = createTaskViewModel.Bounty;
            task.TaskGroup = taskGroup;
            await wDDAContext.Tasks.AddAsync(task);

            taskGroup.Tasks = taskGroup.Tasks.Append(task);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string taskId)
        {
            var task = wDDAContext.Tasks.FirstOrDefault((t) => t.Id == taskId);
            if (task == null)
            {
                return NotFound();
            }

            if (!VerifyIsGroupAdministrator(task.TaskGroup.Group.Id))
            {
                return Unauthorized();
            }

            task.TaskGroup.Tasks = task.TaskGroup.Tasks.Where((t) => t.Id != task.Id);

            wDDAContext.Tasks.Remove(task);
            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
            
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskViewModel updateTaskViewModel)
        {
            var task = wDDAContext.Tasks.FirstOrDefault((t) => t.Id == updateTaskViewModel.TaskId);
            if (task == null)
            {
                return NotFound();
            }

            if (!VerifyIsGroupAdministrator(task.TaskGroup.Group.Id))
            {
                return Unauthorized();
            }

            task.Bounty = updateTaskViewModel.Bounty;
            task.Name = updateTaskViewModel.Name;

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }
    }
}
