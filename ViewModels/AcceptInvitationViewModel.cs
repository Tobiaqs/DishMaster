using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class AcceptInvitationViewModel
    {
        [Required, MinLength(36), MaxLength(36)]
        public string InvitationSecret { get; set; }
    }
}