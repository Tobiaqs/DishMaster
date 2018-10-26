using System.Collections.Generic;
using System.Linq;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.Logic
{
    public class TaskGroupRecordLogic : ITaskGroupRecordLogic
    {
        private int CalculateGainedScore(GroupMember groupMember, IEnumerable<TaskGroupMemberLink> links)
        {
            return links.Where(tgml => tgml.GroupMember == groupMember)
                .Sum(link => !link.Task.IsNeutral ? link.Task.Bounty : 0);
        }

        public bool FillTaskGroupRecord(WDDAContext wDDAContext, TaskGroupRecord taskGroupRecord, CreateTaskGroupRecordViewModel createTaskGroupRecordViewModel)
        {
            var presentGroupMembers = createTaskGroupRecordViewModel.PresentGroupMembersIds.Select(groupMemberId =>
                wDDAContext.GroupMembers.SingleOrDefault(gm => gm.Id == groupMemberId)
            );

            var presentGroupMembersList = presentGroupMembers.OrderBy(gm => gm.Score).ToList();

            if (presentGroupMembers.Contains(null))
            {
                return false;
            }

            taskGroupRecord.PresentGroupMembers = presentGroupMembers.ToHashSet();

            var links = new HashSet<TaskGroupMemberLink>();

            var tasksList = taskGroupRecord.TaskGroup.Tasks.OrderByDescending(t => t.Bounty).ToList();

            while (tasksList.Count() > 0)
            {
                var heaviestTask = tasksList[0];
                tasksList.RemoveAt(0);
                
                var lowestScoringMember = presentGroupMembersList.FirstOrDefault();
                if (lowestScoringMember == null)
                {
                    presentGroupMembersList = presentGroupMembers.OrderBy(gm => gm.Score + CalculateGainedScore(gm, links)).ToList();
                    lowestScoringMember = presentGroupMembersList[0];
                }

                presentGroupMembersList.RemoveAt(0);

                var link = new TaskGroupMemberLink();
                link.Task = heaviestTask;
                link.GroupMember = lowestScoringMember;
                link.TaskGroupRecord = taskGroupRecord;
                links.Add(link);
            }

            taskGroupRecord.TaskGroupMemberLinks = links;

            return true;
       }
    }
}