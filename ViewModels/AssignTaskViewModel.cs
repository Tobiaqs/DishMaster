using DishMaster.Annotations;

namespace DishMaster.ViewModels
{
    public class AssignTaskViewModel
    {
        [IsGuid]
        public string GroupMemberId { get; set; }

        [IsGuid]
        public string TaskId { get; set; }

        [IsGuid]
        public string TaskGroupRecordId { get; set; }
    }
}