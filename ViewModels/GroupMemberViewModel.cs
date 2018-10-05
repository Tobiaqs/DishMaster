using System.Collections.Generic;
using System.Linq;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class GroupMemberViewModel
    {
        private readonly string Id;
        private readonly string FullName;
        private readonly float Score;
        private readonly bool Administrator;

        public GroupMemberViewModel(GroupMember groupMember)
        {
            this.Id = groupMember.Id;
            this.FullName = groupMember.Person.FullName;
            this.Score = groupMember.Score;
            this.Administrator = groupMember.Administrator;
        }
    }
}