using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wie_doet_de_afwas.Models;
using wie_doet_de_afwas.Annotations;
using System.Collections.Generic;
using wie_doet_de_afwas.ViewModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using wie_doet_de_afwas.Logic;

namespace wie_doet_de_afwas.Controllers
{
    public class TaskGroupRecordController : BaseController
    {
        private readonly ITaskGroupRecordLogic taskGroupRecordLogic;

        public TaskGroupRecordController(WDDAContext wDDAContext, ITaskGroupRecordLogic taskGroupRecordLogic) : base(wDDAContext)
        {
            this.taskGroupRecordLogic = taskGroupRecordLogic;
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get([FromQuery, IsGuid] string taskGroupRecordId)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecords
                .Include(tgr => tgr.TaskGroup)
                .ThenInclude(tg => tg.Group)
                .Include(tgr => tgr.PresentGroupMembers)
                .Include(tgr => tgr.TaskGroupMemberLinks)
                .ThenInclude((TaskGroupMemberLink link) => link.Task)
                .Include(tgr => tgr.TaskGroupMemberLinks)
                .ThenInclude((TaskGroupMemberLink link) => link.GroupMember)
                .SingleOrDefault(tgr => tgr.Id == taskGroupRecordId);

            if (taskGroupRecord == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            return SucceededJson(new TaskGroupRecordViewModel(taskGroupRecord));
        }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateTaskGroupRecordViewModel createTaskGroupRecordViewModel)
        {
            var taskGroup = wDDAContext.TaskGroups
                .Include(tg => tg.Group)
                .Include(tg => tg.Tasks)
                .SingleOrDefault(tg => tg.Id == createTaskGroupRecordViewModel.TaskGroupId);

            if (taskGroup == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupMember(taskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            var taskGroupRecord = new TaskGroupRecord();
            taskGroupRecord.TaskGroup = taskGroup;
            taskGroupRecord.Date = System.DateTime.UtcNow;
            
            taskGroupRecordLogic.FillTaskGroupRecord(wDDAContext, taskGroupRecord, createTaskGroupRecordViewModel);

            await wDDAContext.TaskGroupMemberLinks.AddRangeAsync(taskGroupRecord.TaskGroupMemberLinks);
            await wDDAContext.TaskGroupRecords.AddAsync(taskGroupRecord);
            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true,
                TaskGroupRecordId = taskGroupRecord.Id
            });
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AssignTask([FromBody] AssignTaskViewModel assignTaskViewModel)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecords
                .Include(tgr => tgr.TaskGroupMemberLinks)
                    .ThenInclude((TaskGroupMemberLink tgml) => tgml.Task)
                .Include(tgr => tgr.TaskGroup)
                    .ThenInclude(tg => tg.Group)
                .SingleOrDefault(tgr => tgr.Id == assignTaskViewModel.TaskGroupRecordId);

            if (taskGroupRecord == null)
            {
                return NotFoundJson();
            }

            if (taskGroupRecord.Finalized)
            {
                return UnauthorizedJson();
            }

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            var groupMember = wDDAContext.GroupMembers.SingleOrDefault(gm => gm.Id == assignTaskViewModel.GroupMemberId);
            if (groupMember == null)
            {
                return NotFoundJson();
            }

            var link = taskGroupRecord.TaskGroupMemberLinks.Where(tgml => tgml.Task.Id == assignTaskViewModel.TaskId).FirstOrDefault();
            if (link == null)
            {
                return NotFoundJson();
            }

            link.GroupMember = groupMember;

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UnassignTask([FromBody] UnassignTaskViewModel unassignTaskViewModel)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecords
                .Include(tgr => tgr.TaskGroupMemberLinks)
                    .ThenInclude((TaskGroupMemberLink tgml) => tgml.Task)
                .Include(tgr => tgr.TaskGroupMemberLinks)
                    .ThenInclude((TaskGroupMemberLink tgml) => tgml.GroupMember)
                .Include(tgr => tgr.TaskGroup)
                    .ThenInclude(tg => tg.Group)
                .SingleOrDefault(tgr =>
                    tgr.Id == unassignTaskViewModel.TaskGroupRecordId
                );

            if (taskGroupRecord == null)
            {
                return NotFoundJson();
            }

            if (taskGroupRecord.Finalized)
            {
                return UnauthorizedJson();
            }

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            var link = taskGroupRecord.TaskGroupMemberLinks
                .Where(tgml => tgml.Task.Id == unassignTaskViewModel.TaskId)
                .FirstOrDefault();

            if (link == null)
            {
                return NotFoundJson();
            }

            link.GroupMember = null;

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string taskGroupRecordId)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecords
                .Include(tgr => tgr.TaskGroup)
                .ThenInclude(tg => tg.Group)
                .SingleOrDefault(tgr => tgr.Id == taskGroupRecordId);

            if (taskGroupRecord == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            if (taskGroupRecord.Finalized)
            {
                return UnauthorizedJson();
            }

            wDDAContext.TaskGroupRecords.Remove(taskGroupRecord);

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Finalize([FromQuery, IsGuid] string taskGroupRecordId)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecords
                .Include(tgr => tgr.TaskGroup)
                    .ThenInclude(tg => tg.Group)
                .Include(tgr => tgr.PresentGroupMembers)
                .Include(tgr => tgr.TaskGroupMemberLinks)
                    .ThenInclude((TaskGroupMemberLink link) => link.Task)
                .Include(tgr => tgr.TaskGroupMemberLinks)
                    .ThenInclude((TaskGroupMemberLink link) => link.GroupMember)
                .SingleOrDefault(tgr => tgr.Id == taskGroupRecordId);

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            taskGroupRecord.Finalized = true;

            var groupMembers = wDDAContext.GroupMembers.Where(gm =>
                gm.Group == taskGroupRecord.TaskGroup.Group
            );

            double preAverage = groupMembers.Sum(gm => gm.Score) / groupMembers.Count();
            var compensatedGroupMembers = groupMembers.Where(gm => !taskGroupRecord.PresentGroupMembers.Contains(gm)).ToHashSet();

            foreach (var link in taskGroupRecord.TaskGroupMemberLinks)
            {
                if (link.GroupMember != null && link.Task != null)
                {
                    if (link.Task.IsNeutral)
                    {
                        compensatedGroupMembers.Add(link.GroupMember);
                    }
                    else
                    {
                        link.GroupMember.Score += link.Task.Bounty;
                        link.ThenBounty = link.Task.Bounty;
                    }
                }
            }

            await wDDAContext.SaveChangesAsync();

            double postAverage = groupMembers.Sum(gm => gm.Score) / groupMembers.Count();

            foreach (var absentGroupMember in compensatedGroupMembers)
            {
                absentGroupMember.Score += postAverage - preAverage;
            }

            await wDDAContext.SaveChangesAsync();

            return SucceededJson();
        }
    }
}