using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wie_doet_de_afwas.Models;
using wie_doet_de_afwas.Annotations;
using System.Collections.Generic;

namespace wie_doet_de_afwas.Controllers
{
    public class TaskGroupRecordController : TokenAuthBaseController
    {
        public TaskGroupRecordController(WDDAContext wDDAContext) : base(wDDAContext)
        { }

        [HttpPut, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateRecord([FromBody] CreateTaskGroupRecordViewModel createTaskGroupRecordViewModel)
        {
            var taskGroup = wDDAContext.TaskGroups.FirstOrDefault((tg) => tg.Id == createTaskGroupRecordViewModel.TaskGroupId);

            if (taskGroup == null)
            {
                return NotFound();
            }

            if (!VerifyIsGroupMember(taskGroup.Group.Id))
            {
                return Unauthorized();
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

        private int CalculateGainedScore(GroupMember groupMember, TaskGroupRecord taskGroupRecord, IEnumerable<Models.Task> tasks)
        {
            return taskGroupRecord.TaskIdToGroupMemberIdMap.Where((kv) => kv.Value == groupMember.Id)
                .Sum((kv) => tasks.First(t => t.Id == kv.Key).Bounty);
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

            var tasks = taskGroupRecord.TaskGroup.Tasks;
            var tasksList = tasks.OrderByDescending((t) => t.Bounty).ToList();

            while (tasks.Count() > 0)
            {
                var heaviestTask = tasksList[0];
                tasksList.Remove(heaviestTask);
                
                var lowestScoringMember = presentGroupMembers.FirstOrDefault();
                if (lowestScoringMember == null)
                {
                    presentGroupMembers.OrderBy((gm) => gm.Score + CalculateGainedScore(gm, taskGroupRecord, tasks));
                    lowestScoringMember = presentGroupMembers.FirstOrDefault();
                }

                taskGroupRecord.TaskIdToGroupMemberIdMap.Add(heaviestTask.Id, lowestScoringMember.Id);
            }

            return true;
        }

        [HttpPatch, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> FinalizeRecord([FromQuery, IsGuid] string taskGroupRecordId)
        {
            var taskGroupRecord = wDDAContext.TaskGroupRecord.FirstOrDefault((tgr) => tgr.Id == taskGroupRecordId);

            if (!VerifyIsGroupMember(taskGroupRecord.TaskGroup.Group.Id))
            {
                return Unauthorized();
            }

            taskGroupRecord.Finalized = true;

            await wDDAContext.SaveChangesAsync();

            return Json(new {
                Succeeded = true
            });
        }
    }
}