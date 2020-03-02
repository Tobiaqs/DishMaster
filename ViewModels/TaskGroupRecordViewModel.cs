using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class TaskGroupRecordViewModel : SuperficialTaskGroupRecordViewModel
    {
        public TaskGroupRecordViewModel(TaskGroupRecord taskGroupRecord) : base(taskGroupRecord)
        {
            this.PresentGroupMembersIds = taskGroupRecord.PresentGroupMembers.Select(pgm => pgm.GroupMember?.Id); // also deal with removed group members
            this.AssignedTasks = taskGroupRecord.TaskGroupMemberLinks
                .OrderBy(tgml => tgml.Task?.Name.ToLower())
                .Select(link => new AssignedTaskViewModel(link));
        }

        public IEnumerable<string> PresentGroupMembersIds { get; }

        public IEnumerable<AssignedTaskViewModel> AssignedTasks { get; }
    }
}