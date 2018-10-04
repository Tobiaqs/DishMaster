using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class CreateTaskGroupViewModel
    {
        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required, MinLength(36), MaxLength(36)]
        public string GroupId { get; set; }
    }
}