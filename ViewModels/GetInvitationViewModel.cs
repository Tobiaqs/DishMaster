using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class GetInvitationViewModel
    {
        [IsGuid]
        public string GroupId { get; set; }
    }
}