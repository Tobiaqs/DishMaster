using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class UpdateGroupViewModel : CreateGroupViewModel
    {        
        [Required]
        public string GroupId { get; set; }
    }
}