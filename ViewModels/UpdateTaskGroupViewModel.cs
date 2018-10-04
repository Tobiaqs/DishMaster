using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class UpdateTaskGroupViewModel
    {        
        [IsGuid]
        public string TaskGroupId { get; set; }

        [Required, MinLength(1)]
        public string Name { get; set; }
    }
}