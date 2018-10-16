using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wie_doet_de_afwas.Annotations;
using wie_doet_de_afwas.Models;
using wie_doet_de_afwas.ViewModels;

namespace wie_doet_de_afwas.Controllers
{
    public class TaskController : BaseController
    {
        public TaskController(WDDAContext wDDAContext) : base(wDDAContext)
        { }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string taskId)
        {
            var task = wDDAContext.Tasks
                .Include(t => t.TaskGroup)
                .ThenInclude(tg => tg.Group)
                .SingleOrDefault(t => t.Id == taskId);
            
            if (task == null)
            {
                return NotFoundJson();
            }

            var groupMember = wDDAContext.GroupMembers.SingleOrDefault((gm) =>
                gm.Group == task.TaskGroup.Group &&
                gm.Person == GetPerson());

            if (groupMember == null)
            {
                return UnauthorizedJson();
            }

            return SucceededJson(new TaskViewModel(task));
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateTaskViewModel createTaskViewModel)
        {
            var taskGroup = wDDAContext.TaskGroups
                .Include(tg => tg.Group)
                .SingleOrDefault(tg => tg.Id == createTaskViewModel.TaskGroupId);

            if (taskGroup == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(taskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            var task = new Models.Task();
            task.Name = createTaskViewModel.Name;
            task.Bounty = createTaskViewModel.Bounty;
            task.IsNeutral = createTaskViewModel.IsNeutral;
            task.TaskGroup = taskGroup;
            await wDDAContext.Tasks.AddAsync(task);

            taskGroup.Tasks.Add(task);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true,
                TaskId = task.Id
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string taskId)
        {
            var task = wDDAContext.Tasks
                .Include(t => t.TaskGroup)
                .ThenInclude(tg => tg.Group)
                .SingleOrDefault(t => t.Id == taskId);
                
            if (task == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(task.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            task.TaskGroup.Tasks.Remove(task);

            wDDAContext.Tasks.Remove(task);
            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
            
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskViewModel updateTaskViewModel)
        {
            var task = wDDAContext.Tasks
                .Include(t => t.TaskGroup)
                .ThenInclude(tg => tg.Group)
                .SingleOrDefault(t => t.Id == updateTaskViewModel.TaskId);
            if (task == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupAdministrator(task.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            task.Bounty = updateTaskViewModel.Bounty;
            task.Name = updateTaskViewModel.Name;

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }
    }
}
