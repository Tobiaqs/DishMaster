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
            this.GroupMemberId = taskGroupMemberLink.GroupMember?.Id;
        }

        public string RandomId { get; }
        public string TaskId { get; }
        public int ThenBounty { get; }
        public string GroupMemberId { get; }
    }
}