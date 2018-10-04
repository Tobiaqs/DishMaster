using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class AcceptInvitationViewModel
    {
        [IsGuid]
        public string InvitationSecret { get; set; }
    }
}