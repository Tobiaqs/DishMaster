using System.Collections.Generic;
using System.Linq;
using DishMaster.Models;

namespace DishMaster.ViewModels
{
    // View model used for outputting only
    public class GroupViewModel
    {
        public GroupViewModel(Group group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
            this.GroupMembers = group.GroupMembers.OrderBy(gm => gm.IsAnonymous ? gm.AnonymousName : gm.Person.FullName).Select<GroupMember, GroupMemberViewModel>(gm =>
                new GroupMemberViewModel(gm)
            );
        }

        public string Id { get; }
        
        public string Name { get; }

        public IEnumerable<GroupMemberViewModel> GroupMembers { get; }
    }
}