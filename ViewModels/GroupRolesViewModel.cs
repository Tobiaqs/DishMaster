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

        public readonly string GroupMemberId;
        public readonly bool Administrator;
        public readonly bool OnlyAdministrator;
    }
}