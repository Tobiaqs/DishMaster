using System.Collections.Generic;
using System.Linq;
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
            this.TaskIdToGroupMemberIdMap = new Dictionary<string, string>(taskGroupRecord.MappingTasks.Zip(taskGroupRecord.MappingGroupMembers, (task, groupMember) => new KeyValuePair<string, string>(task.Id, groupMember.Id)));
            this.Finalized = taskGroupRecord.Finalized;
        }

        public readonly string Id;

        public readonly System.DateTime Date;

        public readonly IDictionary<string, string> TaskIdToGroupMemberIdMap;

        public readonly bool Finalized;
    }
}