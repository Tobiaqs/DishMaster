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

namespace wie_doet_de_afwas.Controllers
{
    public class TaskGroupRecordController : BaseController
    {
        public TaskGroupRecordController(WDDAContext wDDAContext) : base(wDDAContext)
        { }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateTaskGroupRecordViewModel createTaskGroupRecordViewModel)
        {
            var taskGroup = wDDAContext.TaskGroups.FirstOrDefault((tg) => tg.Id == createTaskGroupRecordViewModel.TaskGroupId);

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
            
            FillTaskGroupRecord(taskGroupRecord, createTaskGroupRecordViewModel);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true,
                TaskGroupRecordId = taskGroupRecord.Id
            });

        }

        private int CalculateGainedScore(GroupMember groupMember, Dictionary<Models.Task, GroupMember> taskIdGroupMemberIdMap, IEnumerable<Models.Task> tasks)
        {
            return taskIdGroupMemberIdMap.Where((kv) => kv.Value == groupMember)
                .Sum((kv) => tasks.First(t => t == kv.Key).Bounty);
        }

        private bool FillTaskGroupRecord(TaskGroupRecord taskGroupRecord, CreateTaskGroupRecordViewModel createTaskGroupRecordViewModel)
        {
            var presentGroupMembers = createTaskGroupRecordViewModel.PresentGroupMemberIds.Select((groupMemberId) =>
                wDDAContext.GroupMembers.FirstOrDefault((gm) => gm.Id == groupMemberId)
            );

            var presentGroupMembersList = presentGroupMembers.OrderBy((gm) => gm.Score).ToList();

            if (presentGroupMembers.Contains(null))
            {
                return false;
            }

            taskGroupRecord.PresentGroupMembers = presentGroupMembers;

            var mapping = new Dictionary<Models.Task, GroupMember>();

            var tasks = taskGroupRecord.TaskGroup.Tasks;
            var tasksList = tasks.OrderByDescending((t) => t.Bounty).ToList();

            while (tasksList.Count() > 0)
            {
                var heaviestTask = tasksList[0];
                tasksList.RemoveAt(0);
                
                var lowestScoringMember = presentGroupMembers.FirstOrDefault();
                if (lowestScoringMember == null)
                {
                    presentGroupMembersList = presentGroupMembers.OrderBy((gm) => gm.Score + CalculateGainedScore(gm, mapping, tasks)).ToList();
                    lowestScoringMember = presentGroupMembersList[0];
                    presentGroupMembersList.RemoveAt(0);
                }

                mapping.Add(heaviestTask, lowestScoringMember);
            }

            taskGroupRecord.MappingTasks = mapping.Keys.ToList();
            taskGroupRecord.MappingGroupMembers = mapping.Values.ToList();

            return true;
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AssignTask([FromBody] AssignTaskViewModel assignTaskViewModel)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecord.FirstOrDefault((tgr) =>
                tgr.Id == assignTaskViewModel.TaskGroupRecordId
            );

            if (taskGroupRecord == null)
            {
                return NotFoundJson();
            }

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            var groupMember = wDDAContext.GroupMembers.FirstOrDefault((gm) => gm.Id == assignTaskViewModel.GroupMemberId);
            
            int groupMemberIdx = taskGroupRecord.MappingGroupMembers.IndexOf(groupMember);

            if (groupMemberIdx == -1)
            {
                return NotFoundJson();
            }

            var task = wDDAContext.Tasks.FirstOrDefault((t) => t.Id == assignTaskViewModel.TaskId);

            if (task == null)
            {
                return NotFoundJson();
            }

            taskGroupRecord.MappingTasks[groupMemberIdx] = task;

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UnassignTask([FromBody] UnassignTaskViewModel unassignTaskViewModel)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecord.FirstOrDefault((tgr) => tgr.Id == unassignTaskViewModel.TaskGroupRecordId);

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            var task = wDDAContext.Tasks.FirstOrDefault((t) => t.Id == unassignTaskViewModel.TaskId);

            if (task == null)
            {
                return NotFoundJson();
            }

            var taskIdx = taskGroupRecord.MappingTasks.IndexOf(task);

            taskGroupRecord.MappingGroupMembers[taskIdx] = null;

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromQuery, IsGuid] string taskGroupRecordId)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecord.FirstOrDefault((tgr) => tgr.Id == taskGroupRecordId);

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

            wDDAContext.TaskGroupRecord.Remove(taskGroupRecord);

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Finalize([FromQuery, IsGuid] string taskGroupRecordId)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecord.FirstOrDefault((tgr) => tgr.Id == taskGroupRecordId);

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return UnauthorizedJson();
            }

            taskGroupRecord.Finalized = true;
            taskGroupRecord.Date = System.DateTime.UtcNow;

            var groupMembers = wDDAContext.GroupMembers.Where((gm) =>
                gm.Group == taskGroupRecord.TaskGroup.Group
            );

            float preAverage = groupMembers.Sum((gm) => gm.Score) / groupMembers.Count();

            var absentGroupMembers = groupMembers.Where((gm) => !taskGroupRecord.PresentGroupMembers.Contains(gm));

            IEnumerable<KeyValuePair<Models.Task, GroupMember>> zippedMapping = taskGroupRecord.MappingTasks.Zip(taskGroupRecord.MappingGroupMembers, (t, tgr) => new KeyValuePair<Models.Task, GroupMember>(t, tgr));

            foreach (var pair in zippedMapping)
            {
                if (pair.Value != null)
                {
                    pair.Value.Score += pair.Key.Bounty;
                }
            }

            float postAverage = groupMembers.Sum((gm) => gm.Score) / groupMembers.Count();

            foreach (var absentGroupMember in absentGroupMembers)
            {
                absentGroupMember.Score += postAverage - preAverage;
            }

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }
    }
}