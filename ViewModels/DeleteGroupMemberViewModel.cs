using System.ComponentModel.DataAnnotations.Schema;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class DeleteGroupMemberViewModel
    {
        [IsGuid]
        public string GroupMemberId;
    }
}