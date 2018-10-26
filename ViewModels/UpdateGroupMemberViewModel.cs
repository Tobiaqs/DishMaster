using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class UpdateGroupMemberViewModel
    {
        [IsGuid]
        public string GroupMemberId { get; set; }
        
        public bool AbsentByDefault { get; set; }
    }
}