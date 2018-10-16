using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class AddAnonymousGroupMemberViewModel
    {
        [IsValidName]
        public string AnonymousName { get; set; }

        [IsGuid]
        public string GroupId { get; set; }
    }
}