using DishMaster.Annotations;

namespace DishMaster.ViewModels
{
    public class UpdateGroupMemberViewModel
    {
        [IsGuid]
        public string GroupMemberId { get; set; }
        
        public bool AbsentByDefault { get; set; }
    }
}