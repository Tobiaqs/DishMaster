using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class UpdateGroupViewModel : CreateGroupViewModel
    {        
        [Required, MinLength(36), MaxLength(36)]
        public string GroupId { get; set; }
    }
}