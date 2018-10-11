using System.Collections.Generic;
using System.Linq;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class GroupMemberViewModel
    {
        public readonly string Id;
        public readonly string FullName;
        public readonly string AnonymousName;
        public readonly bool IsAnonymous;
        public readonly float Score;
        public readonly bool Administrator;

        public GroupMemberViewModel(GroupMember groupMember)
        {
            this.Id = groupMember.Id;
            this.FullName = groupMember.Person?.FullName;
            this.AnonymousName = groupMember.AnonymousName;
            this.IsAnonymous = groupMember.IsAnonymous;
            this.Score = groupMember.Score;
            this.Administrator = groupMember.Administrator;
        }
    }
}