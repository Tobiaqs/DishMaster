using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class UpdateTaskViewModel
    {
        [IsGuid]
        public string TaskId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Bounty { get; set; }
    }
}