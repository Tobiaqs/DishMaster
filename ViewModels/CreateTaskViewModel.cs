using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class CreateTaskViewModel
    {
        [Required]
        public string Name { get; set; }

        public int Bounty { get; set; }

        public bool IsNeutral { get; set; }

        [IsGuid]
        public string TaskGroupId { get; set; }
    }
}