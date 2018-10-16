using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class CreateGroupViewModel
    {
        [IsValidName]
        public string Name { get; set; }
    }
}