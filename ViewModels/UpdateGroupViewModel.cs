using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class UpdateGroupViewModel
    {
        [IsGuid]
        public string GroupId { get; set; }

        [Required, MinLength(1)]
        public string Name { get; set; }
    }
}