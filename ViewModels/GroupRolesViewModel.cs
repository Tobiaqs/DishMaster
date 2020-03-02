using wie_doet_de_afwas.Models;
using System.Linq;

namespace wie_doet_de_afwas.ViewModels
{
    public class GroupRolesViewModel {
        public GroupRolesViewModel(GroupMember groupMember)
        {
            this.GroupMemberId = groupMember.Id;
            this.Administrator = groupMember.Administrator;
            this.OnlyAdministrator = !groupMember.Group.GroupMembers.Any(gm => gm.Administrator && gm != groupMember);
        }

        public string GroupMemberId { get; }
        public bool Administrator { get; }
        public bool OnlyAdministrator { get; }
    }
}