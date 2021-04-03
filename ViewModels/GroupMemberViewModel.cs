using DishMaster.Models;

namespace DishMaster.ViewModels
{
    // View model used for outputting only
    public class GroupMemberViewModel
    {
        public string Id { get; }
        public string FullName { get; }
        public string AnonymousName { get; }
        public bool IsAnonymous { get; }
        public double Score { get; }
        public bool Administrator { get; }
        public bool AbsentByDefault { get; }

        public GroupMemberViewModel(GroupMember groupMember)
        {
            this.Id = groupMember.Id;
            this.FullName = groupMember.Person?.FullName;
            this.AnonymousName = groupMember.AnonymousName;
            this.IsAnonymous = groupMember.IsAnonymous;
            this.Score = groupMember.Score;
            this.Administrator = groupMember.Administrator;
            this.AbsentByDefault = groupMember.AbsentByDefault;
        }
    }
}