using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class CreateGroupViewModel
    {
        [Required, MinLength(1)]
        public string Name { get; set; }
    }
}