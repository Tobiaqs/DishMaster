using System.Collections.Generic;
using System.Linq;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class GroupViewModel
    {
        public GroupViewModel(Group group, IEnumerable<GroupMember> groupMembers)
        {
            this.Id = group.Id;
            this.Name = group.Name;
            this.GroupMembers = groupMembers.Select<GroupMember, GroupMemberViewModel>((gm) =>
                new GroupMemberViewModel(gm)
            );
        }

        public readonly string Id;
        
        public readonly string Name;

        public IEnumerable<GroupMemberViewModel> GroupMembers { get; set; }
    }
}