using System.Collections.Generic;
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
            this.TaskIdToGroupMemberIdMap = taskGroupRecord.TaskIdToGroupMemberIdMap;
            this.Finalized = taskGroupRecord.Finalized;
        }

        public readonly string Id;

        public readonly System.DateTime Date;

        public readonly IDictionary<string, string> TaskIdToGroupMemberIdMap;

        public readonly bool Finalized;
    }
}