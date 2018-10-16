using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;
using wie_doet_de_afwas.Logic;

namespace wie_doet_de_afwas.ViewModels
{
    public class CreateTaskViewModel
    {
        [IsValidName]
        public string Name { get; set; }

        [IsValidBounty]
        public int Bounty { get; set; }

        public bool IsNeutral { get; set; }

        [IsGuid]
        public string TaskGroupId { get; set; }
    }
}