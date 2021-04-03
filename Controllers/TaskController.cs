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
    public class TaskController : BaseController
    {
        public TaskController(DMContext dMContext) : base(dMContext)
        { }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string taskId)
        {
            var task = dMContext.Tasks
                .Include(t => t.TaskGroup)
                .ThenInclude(tg => tg.Group)
                .SingleOrDefault(t => t.Id == taskId);
            
            if (task == null)
            {
                return NotFoundJson();
            }

            var groupMember = dMContext.GroupMembers.SingleOrDefault((gm) =>
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
            var taskGroup = dMContext.TaskGroups
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
            await dMContext.Tasks.AddAsync(task);

            taskGroup.Tasks.Add(task);

            await dMContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true,
                TaskId = task.Id
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string taskId)
        {
            var task = dMContext.Tasks
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

            dMContext.Tasks.Remove(task);
            await dMContext.SaveChangesAsync();

            return SucceededJson();
            
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskViewModel updateTaskViewModel)
        {
            var task = dMContext.Tasks
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

            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }
    }
}
