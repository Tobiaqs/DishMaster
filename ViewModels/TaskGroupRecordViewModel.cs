using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class TaskGroupRecordViewModel
    {
        public TaskGroupRecordViewModel(TaskGroupRecord taskGroupRecord)
        {
            this.Id = taskGroupRecord.Id;
            this.Date = taskGroupRecord.Date;
            this.PresentGroupMembersIds = taskGroupRecord.PresentGroupMembers.Select(gm => gm.Id);
            this.AssignedTasks = taskGroupRecord.TaskGroupMemberLinks
                .OrderBy(tgml => tgml.Task?.Name.ToLower())
                .Select(link => new AssignedTaskViewModel(link));
            this.Finalized = taskGroupRecord.Finalized;
        }

        public readonly string Id;

        public readonly System.DateTime Date;

        public readonly IEnumerable<string> PresentGroupMembersIds;

        public readonly IEnumerable<AssignedTaskViewModel> AssignedTasks;

        public readonly bool Finalized;
    }
}