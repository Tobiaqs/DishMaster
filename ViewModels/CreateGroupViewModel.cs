using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class CreateGroupViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}