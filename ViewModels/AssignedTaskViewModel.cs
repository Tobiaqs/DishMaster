using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class AssignedTaskViewModel
    {
        public AssignedTaskViewModel(TaskGroupMemberLink taskGroupMemberLink)
        {
            this.RandomId = System.Guid.NewGuid().ToString();
            this.TaskId = taskGroupMemberLink.Task?.Id;
            this.ThenBounty = taskGroupMemberLink.ThenBounty;
            this.GroupMemberId = taskGroupMemberLink.GroupMember.Id;
        }

        public readonly string RandomId;
        public readonly string TaskId;
        public readonly int ThenBounty;
        public readonly string GroupMemberId;
    }
}