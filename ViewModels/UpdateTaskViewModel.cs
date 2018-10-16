using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;
using wie_doet_de_afwas.Logic;

namespace wie_doet_de_afwas.ViewModels
{
    public class UpdateTaskViewModel
    {
        [IsGuid]
        public string TaskId { get; set; }

        [IsValidName]
        public string Name { get; set; }

        [IsValidBounty]
        public int Bounty { get; set; }
    }
}